
using OrderApi.Models;
using OrderApi.Enums;

namespace OrderApi.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(Guid id);
        Task CreateOrderAsync(Order order);
        Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
        Task<Order> UpdateOrderAsync(Order order, OrderDto orderDto);
        Task DeleteOrderAsync(Guid id);
    }
}