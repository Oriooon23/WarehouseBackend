using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Context;
using Warehouse.Data.Models;
using Warehouse.Interfaces.RepositoryInterfaces;
using Warehouse.Repositories.Repositories;
using System.Threading.Tasks;

namespace Warehouse.Repositories.Repositories
{
    public class OrderRepository : GenericRepository<Orders>, IOrderRepository
    {
        public OrderRepository(WarehouseDbContext context) : base(context)
        { }
    }
}