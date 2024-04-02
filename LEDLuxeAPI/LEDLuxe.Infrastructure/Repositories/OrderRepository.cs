using LEDLuxe.Core.Entities.Exceptions;
using LEDLuxe.Core.Entities.Orders;
using LEDLuxe.Core.Entities.Products;
using LEDLuxe.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LEDLuxe.Infrastructure.Repositories;

public class OrderRepository(ApplicationDbContext context) : IOrderRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Order> GetByIdAsync(Guid id)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.UserId == userId)
            .ToListAsync();
    }

    public async Task AddOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateOrderAsync(Order order)
    {
        try
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConcurrencyConflictException("A concurrency conflict occurred while updating the order.");
        }
    }

    public async Task DeleteOrderAsync(Guid orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);

        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddItemAsync(Guid orderId, Product product, int quantity)
    {
        var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == orderId);

        if (order != null)
        {
            order.AddItem(product, quantity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveItemAsync(Guid orderId, Guid productId)
    {
        var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == orderId);

        if (order != null)
        {
            order.RemoveItem(productId);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateItemQuantityAsync(Guid orderId, Guid productId, int quantity)
    {
        var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == orderId);

        if (order != null)
        {
            order.UpdateItemQuantity(productId, quantity);
            await _context.SaveChangesAsync();
        }
    }
}