using Warehouse.Data.Models;

namespace Warehouse.Interfaces.RepositoryInterfaces
{
    public interface IOrderStatusRepository : IGenericRepository<OrderStatuses>
    {
        Task<OrderStatuses> GetOrderStatusByNameAsync(string statusName);
    }
}