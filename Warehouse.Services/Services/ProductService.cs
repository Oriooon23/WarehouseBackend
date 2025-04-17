using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;
using Warehouse.Data.Models;
using Warehouse.Interfaces.RepositoryInterfaces;
using Warehouse.Interfaces.ServicesInterfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Warehouse.Common.Security;

namespace Warehouse.Services.Services
{
    public class ProductService : GenericService<Products, ProductDTO>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(IProductRepository productRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(productRepository, mapper)
        {
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseBase<ProductDTO>> CreateProductAsync(ProductDTO productDto)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user != null && (user.IsInRole(Policies.Admin) || user.IsInRole(Policies.Supplier)))
            {
                try
                {
                    var productEntity = _mapper.Map<Products>(productDto);
                    var addedProduct = await _productRepository.AddAsync(productEntity);
                    var addedProductDto = _mapper.Map<ProductDTO>(addedProduct);
                    return ResponseBase<ProductDTO>.Success(addedProductDto);
                }
                catch (Exception ex)
                {
                    return ResponseBase<ProductDTO>.Fail($"Errore durante la creazione del prodotto: {ex.Message}", ErrorCode.InternalError);
                }
            }
            else
            {
                return ResponseBase<ProductDTO>.Fail("Non autorizzato a creare prodotti.", ErrorCode.Unauthorized);
            }
        }

        public async Task<ResponseBase<ProductDTO>> UpdateProductAsync(int id, ProductDTO productDto)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user != null && (user.IsInRole(Policies.Admin) || user.IsInRole(Policies.Supplier)))
            {
                try
                {
                    var existingProduct = await _productRepository.GetByIdAsync(id);
                    if (existingProduct == null)
                    {
                        return ResponseBase<ProductDTO>.Fail("Prodotto non trovato.", ErrorCode.NotFound);
                    }

                    _mapper.Map(productDto, existingProduct);
                    await _productRepository.UpdateAsync(existingProduct);
                    var updatedProductDto = _mapper.Map<ProductDTO>(existingProduct);
                    return ResponseBase<ProductDTO>.Success(updatedProductDto);
                }
                catch (Exception ex)
                {
                    return ResponseBase<ProductDTO>.Fail($"Errore durante l'aggiornamento del prodotto: {ex.Message}", ErrorCode.InternalError);
                }
            }
            else
            {
                return ResponseBase<ProductDTO>.Fail("Non autorizzato a modificare prodotti.", ErrorCode.Unauthorized);
            }
        }

        public async Task<ResponseBase<bool>> SoftDeleteProductAsync(int id)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user != null && (user.IsInRole(Policies.Admin) || user.IsInRole(Policies.Supplier)))
            {
                try
                {
                    var productToDelete = await _productRepository.GetByIdAsync(id);
                    if (productToDelete == null)
                    {
                        return ResponseBase<bool>.Fail("Prodotto non trovato.", ErrorCode.NotFound);
                    }

                    productToDelete.StockQuantity = 0;
                    await _productRepository.UpdateAsync(productToDelete);
                    return ResponseBase<bool>.Success(true);
                }
                catch (Exception ex)
                {
                    return ResponseBase<bool>.Fail($"Errore durante l'eliminazione del prodotto: {ex.Message}", ErrorCode.InternalError);
                }
            }
            else
            {
                return ResponseBase<bool>.Fail("Non autorizzato a eliminare prodotti.", ErrorCode.Unauthorized);
            }
        }

        public async Task<ResponseBase<IEnumerable<ProductDTO>>> SearchProductsAsync(string categoryName = null, string productName = null, string supplierName = null)
        {
            try
            {
                var products = await _productRepository.SearchProductsAsync(categoryName, productName, supplierName);
                var productDtos = _mapper.Map<IEnumerable<ProductDTO>>(products);
                return ResponseBase<IEnumerable<ProductDTO>>.Success(productDtos);
            }
            catch (Exception ex)
            {
                return ResponseBase<IEnumerable<ProductDTO>>.Fail($"Errore durante la ricerca dei prodotti: {ex.Message}", ErrorCode.InternalError);
            }
        }
    }
}