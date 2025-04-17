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
    public class SupplierService : GenericService<Suppliers, SupplierDTO>, ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SupplierService(ISupplierRepository supplierRepository, IProductRepository productRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(supplierRepository, mapper)
        {
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseBase<SupplierDTO>> CreateSupplierAsync(SupplierDTO supplierDto)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user != null && user.IsInRole(Policies.Admin))
            {
                try
                {
                    var supplierEntity = _mapper.Map<Suppliers>(supplierDto);
                    var addedSupplier = await _supplierRepository.AddAsync(supplierEntity);
                    var addedSupplierDto = _mapper.Map<SupplierDTO>(addedSupplier);
                    return ResponseBase<SupplierDTO>.Success(addedSupplierDto);
                }
                catch (Exception ex)
                {
                    return ResponseBase<SupplierDTO>.Fail($"Errore durante la creazione del fornitore: {ex.Message}", ErrorCode.InternalError);
                }
            }
            else
            {
                return ResponseBase<SupplierDTO>.Fail("Non autorizzato a creare fornitori.", ErrorCode.Unauthorized);
            }
        }

        public async Task<ResponseBase<SupplierDTO>> UpdateSupplierAsync(int id, SupplierDTO supplierDto)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user != null && (user.IsInRole(Policies.Admin) || user.IsInRole(Policies.Supplier)))
            {
                try
                {
                    var existingSupplier = await _supplierRepository.GetByIdAsync(id);
                    if (existingSupplier == null)
                    {
                        return ResponseBase<SupplierDTO>.Fail("Fornitore non trovato.", ErrorCode.NotFound);
                    }

                    _mapper.Map(supplierDto, existingSupplier);
                    await _supplierRepository.UpdateAsync(existingSupplier);
                    var updatedSupplierDto = _mapper.Map<SupplierDTO>(existingSupplier);
                    return ResponseBase<SupplierDTO>.Success(updatedSupplierDto);
                }
                catch (Exception ex)
                {
                    return ResponseBase<SupplierDTO>.Fail($"Errore durante l'aggiornamento del fornitore: {ex.Message}", ErrorCode.InternalError);
                }
            }
            else
            {
                return ResponseBase<SupplierDTO>.Fail("Non autorizzato a modificare fornitori.", ErrorCode.Unauthorized);
            }
        }

        public async Task<ResponseBase<bool>> DeleteSupplierAsync(int id)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user != null && user.IsInRole(Policies.Admin))
            {
                try
                {
                    var supplierToDelete = await _supplierRepository.GetByIdAsync(id);
                    if (supplierToDelete == null)
                    {
                        return ResponseBase<bool>.Fail("Fornitore non trovato.", ErrorCode.NotFound);
                    }

                    var productsToUpdate = await _productRepository.GetAllAsync();
                    if (productsToUpdate != null)
                    {
                        foreach (var product in productsToUpdate.Where(p => p.IdSupplier == id))
                        {
                            product.StockQuantity = 0;
                            await _productRepository.UpdateAsync(product);
                        }
                    }
                    else
                    {
                        return ResponseBase<bool>.Fail("Errore durante il recupero dei prodotti.", ErrorCode.InternalError);
                    }

                    await _supplierRepository.DeleteAsync(supplierToDelete.Id);
                    return ResponseBase<bool>.Success(true);
                }
                catch (Exception ex)
                {
                    return ResponseBase<bool>.Fail($"Errore durante l'eliminazione del fornitore: {ex.Message}", ErrorCode.InternalError);
                }
            }
            else
            {
                return ResponseBase<bool>.Fail("Non autorizzato a eliminare fornitori.", ErrorCode.Unauthorized);
            }
        }
    }
}