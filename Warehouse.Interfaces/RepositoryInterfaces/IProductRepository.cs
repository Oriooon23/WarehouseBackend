using Warehouse.Data.Models;

namespace Warehouse.Interfaces.RepositoryInterfaces
{
    public interface IProductRepository : IGenericRepository<Products>
    {
        Task<IEnumerable<Products>> SearchProductsAsync(string categoryName = null, string productName = null, string supplierName = null);
    }
}