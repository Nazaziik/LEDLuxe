using LEDLuxe.Core.Entities.Orders;

namespace LEDLuxe.Core.Interfaces.Services;

public interface IOrderService
{
    Task AddItemToOrderAsync(Guid orderId, Guid productId, int quantity);

    Task DeleteOrderAsync(Guid orderId);

    Task<IEnumerable<Order>> GetAllOrdersAsync();

    Task<Order> GetOrderByIdAsync(Guid orderId);

    Task<Order> GetOrderDetailsAsync(Guid orderId);

    Task<IEnumerable<Order>> GetOrderHistoryAsync(Guid userId);

    Task<bool> PlaceOrderAsync(Order order);

    Task RemoveItemFromOrderAsync(Guid orderId, Guid productId);

    Task UpdateOrderAsync(Order updatedOrder);
}