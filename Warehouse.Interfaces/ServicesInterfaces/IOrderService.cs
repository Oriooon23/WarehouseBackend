using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;

namespace Warehouse.Interfaces.ServicesInterfaces
{
    public interface IOrderService : IGenericService<OrderDTO>
    {
        Task<ResponseBase<OrderDTO>> CreateOrderAsync(OrderDTO orderDto);
        Task<ResponseBase<OrderDTO>> UpdateOrderStatusAsync(int id);
    }
}
