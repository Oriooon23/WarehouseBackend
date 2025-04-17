using Warehouse.Data.Models;
using Warehouse.Data.Context;
using Warehouse.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Warehouse.Repositories.Repositories
{
    public class ProductRepository : GenericRepository<Products>, IProductRepository
    {
        public ProductRepository(WarehouseDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Products>> SearchProductsAsync(string categoryName = null, string productName = null, string supplierName = null)
        {
            IQueryable<Products> query = _context.Products.Include(p => p.Category).Include(p => p.Supplier);

            if (!string.IsNullOrEmpty(categoryName))
            {
                query = query.Where(p => p.Category.Name.ToLower().Contains(categoryName.ToLower()));
            }

            if (!string.IsNullOrEmpty(productName))
            {
                query = query.Where(p => p.Name.ToLower().Contains(productName.ToLower()));
            }

            if (!string.IsNullOrEmpty(supplierName))
            {
                query = query.Where(p => p.Supplier.Name.ToLower().Contains(supplierName.ToLower()));
            }

            return await query.ToListAsync();
        }
    }
}