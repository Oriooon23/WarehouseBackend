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
    public class OrderService : GenericService<Orders, OrderDTO>, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderStatusRepository _orderStatusRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IOrderStatusRepository orderStatusRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(orderRepository, mapper)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _orderStatusRepository = orderStatusRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseBase<OrderDTO>> CreateOrderAsync(OrderDTO orderDto)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user != null && (user.IsInRole(Policies.User) || user.IsInRole(Policies.Supplier)))
            {
                try
                {
                    var orderEntity = _mapper.Map<Orders>(orderDto);
                    orderEntity.OrderDate = DateTime.UtcNow;

                    var pendingStatus = await _orderStatusRepository.GetOrderStatusByNameAsync("pending");
                    if (pendingStatus != null)
                    {
                        orderEntity.IdStatus = pendingStatus.Id;
                    }
                    else
                    {
                        return ResponseBase<OrderDTO>.Fail("Stato 'pending' non trovato.", ErrorCode.InternalError);
                    }

                    if (orderDto.OrderProducts != null && orderDto.OrderProducts.Any())
                    {
                        foreach (var orderProductDto in orderDto.OrderProducts)
                        {
                            var product = await _productRepository.GetByIdAsync(orderProductDto.IdProduct);
                            if (product == null)
                            {
                                return ResponseBase<OrderDTO>.Fail($"Prodotto con ID {orderProductDto.IdProduct} non trovato.", ErrorCode.NotFound);
                            }

                            if (product.StockQuantity < orderProductDto.Quantity)
                            {
                                return ResponseBase<OrderDTO>.Fail($"Quantità insufficiente per il prodotto {product.Name}.", ErrorCode.InvalidInput);
                            }

                            product.StockQuantity -= orderProductDto.Quantity;
                            await _productRepository.UpdateAsync(product);
                        }
                    }

                    var addedOrder = await _orderRepository.AddAsync(orderEntity);
                    var addedOrderDto = _mapper.Map<OrderDTO>(addedOrder);
                    return ResponseBase<OrderDTO>.Success(addedOrderDto);
                }
                catch (Exception ex)
                {
                    return ResponseBase<OrderDTO>.Fail($"Errore durante la creazione dell'ordine: {ex.Message}", ErrorCode.InternalError);
                }
            }
            else
            {
                return ResponseBase<OrderDTO>.Fail("Non autorizzato a creare ordini.", ErrorCode.Unauthorized);
            }
        }

        public async Task<ResponseBase<OrderDTO>> UpdateOrderStatusAsync(int id)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user != null && (user.IsInRole(Policies.User) || user.IsInRole(Policies.Supplier)))
            {
                try
                {
                    var orderToCancel = await _orderRepository.GetByIdAsync(id);
                    if (orderToCancel == null)
                    {
                        return ResponseBase<OrderDTO>.Fail("Ordine non trovato.", ErrorCode.NotFound);
                    }

                    if (orderToCancel.Status?.Name != "pending" && orderToCancel.Status?.Name != "confirmed" && orderToCancel.Status?.Name != "delivered")
                    {
                        var cancelledStatus = await _orderStatusRepository.GetOrderStatusByNameAsync("cancelled");
                        if (cancelledStatus != null)
                        {
                            orderToCancel.IdStatus = cancelledStatus.Id;

                            if (orderToCancel.OrderProducts != null && orderToCancel.OrderProducts.Any())
                            {
                                foreach (var orderProduct in orderToCancel.OrderProducts)
                                {
                                    var product = await _productRepository.GetByIdAsync(orderProduct.IdProduct);
                                    if (product != null)
                                    {
                                        product.StockQuantity += orderProduct.Quantity;
                                        await _productRepository.UpdateAsync(product);
                                    }
                                }
                            }

                            await _orderRepository.UpdateAsync(orderToCancel);
                            return ResponseBase<OrderDTO>.Success(_mapper.Map<OrderDTO>(orderToCancel));
                        }
                        else
                        {
                            return ResponseBase<OrderDTO>.Fail("Stato 'cancelled' non trovato.", ErrorCode.InternalError);
                        }
                    }
                    else
                    {
                        return ResponseBase<OrderDTO>.Fail("Impossibile annullare un ordine con lo stato attuale.", ErrorCode.InvalidInput);
                    }
                }
                catch (Exception ex)
                {
                    return ResponseBase<OrderDTO>.Fail($"Errore durante l'annullamento dell'ordine: {ex.Message}", ErrorCode.InternalError);
                }
            }
            else
            {
                return ResponseBase<OrderDTO>.Fail("Non autorizzato ad annullare ordini.", ErrorCode.Unauthorized);
            }
        }
    }
}