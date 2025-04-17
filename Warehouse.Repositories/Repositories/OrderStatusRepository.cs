using Warehouse.Data.Context;
using Warehouse.Data.Models;
using Warehouse.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Warehouse.Repositories.Repositories
{
    public class OrderStatusRepository : GenericRepository<OrderStatuses>, IOrderStatusRepository
    {
        public OrderStatusRepository(WarehouseDbContext context) : base(context)
        {
        }

        public async Task<OrderStatuses> GetOrderStatusByNameAsync(string statusName)
        {
            return await _context.OrderStatuses
                .Where(s => s.Name.ToLower() == statusName.ToLower())
                .FirstOrDefaultAsync();
        }
    }
}