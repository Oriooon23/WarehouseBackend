using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;

namespace Warehouse.Interfaces.ServicesInterfaces
{
    public interface IProductService : IGenericService<ProductDTO>
    {
        Task<ResponseBase<ProductDTO>> CreateProductAsync(ProductDTO productDto);
        Task<ResponseBase<ProductDTO>> UpdateProductAsync(int id, ProductDTO productDto);
        Task<ResponseBase<bool>> SoftDeleteProductAsync(int id);
        Task<ResponseBase<IEnumerable<ProductDTO>>> SearchProductsAsync(string categoryName = null, string productName = null, string supplierName = null);
    }
}