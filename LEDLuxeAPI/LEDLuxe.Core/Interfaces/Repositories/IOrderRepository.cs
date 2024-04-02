using LEDLuxe.Core.Entities.Orders;
using LEDLuxe.Core.Entities.Products;

namespace LEDLuxe.Core.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<Order> GetByIdAsync(Guid id);

    Task<IEnumerable<Order>> GetAllOrdersAsync();

    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);

    Task AddOrderAsync(Order order);

    Task UpdateOrderAsync(Order order);

    Task DeleteOrderAsync(Guid orderId);

    Task AddItemAsync(Guid orderId, Product product, int quantity);

    Task RemoveItemAsync(Guid orderId, Guid productId);

    Task UpdateItemQuantityAsync(Guid orderId, Guid productId, int quantity);
}