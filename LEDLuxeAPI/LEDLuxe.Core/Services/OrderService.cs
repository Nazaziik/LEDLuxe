using LEDLuxe.Core.Entities.Exceptions;
using LEDLuxe.Core.Entities.Orders;
using LEDLuxe.Core.Interfaces;
using LEDLuxe.Core.Interfaces.Repositories;
using LEDLuxe.Core.Interfaces.Services;

namespace LEDLuxe.Core.Services;

public class OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IUnitOfWork unitOfWork) : IOrderService
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Order> GetOrderByIdAsync(Guid orderId)
    {
        return await _orderRepository.GetByIdAsync(orderId);
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllOrdersAsync();
    }

    public async Task<IEnumerable<Order>> GetOrderHistoryAsync(Guid userId)
    {
        return await _orderRepository.GetOrdersByUserIdAsync(userId);
    }

    public async Task<Order> GetOrderDetailsAsync(Guid orderId)
    {
        return await _orderRepository.GetByIdAsync(orderId);
    }

    public async Task<bool> PlaceOrderAsync(Order order)
    {
        if (order == null || order.OrderItems.Count == 0)
            throw new ArgumentException("Order is invalid or empty.");

        using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);

                if (product == null || item.Quantity > product.StockQuantity)
                    throw new InvalidOperationException("Product is unavailable or stock is insufficient.");

                product.StockQuantity -= item.Quantity;
                await _productRepository.UpdateAsync(product);
            }

            await _orderRepository.AddOrderAsync(order);

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteOrderAsync(Guid orderId)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var order = await _orderRepository.GetByIdAsync(orderId) ??
                throw new InvalidOperationException("Order does not exist.");

            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);

                if (product != null)
                {
                    product.StockQuantity += item.Quantity;
                    await _productRepository.UpdateAsync(product);
                }
            }

            await _orderRepository.DeleteOrderAsync(orderId);
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task AddItemToOrderAsync(Guid orderId, Guid productId, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0.");

        using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var order = await _orderRepository.GetByIdAsync(orderId) ??
                throw new InvalidOperationException("Order does not exist.");

            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null || product.StockQuantity < quantity)
                throw new InvalidOperationException("Product is unavailable or stock is insufficient.");

            product.StockQuantity -= quantity;
            await _productRepository.UpdateAsync(product);

            var orderItem = order.OrderItems.FirstOrDefault(item => item.ProductId == productId);

            if (orderItem != null)
                orderItem.IncreaseQuantity(quantity);
            else
                order.OrderItems.Add(new OrderItem(order.Id, productId, product.Price, quantity));

            await _orderRepository.UpdateOrderAsync(order);
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task RemoveItemFromOrderAsync(Guid orderId, Guid productId)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var order = await _orderRepository.GetByIdAsync(orderId) ??
                throw new InvalidOperationException("Order does not exist.");

            var orderItem = order.OrderItems.FirstOrDefault(item => item.ProductId == productId) ??
                throw new InvalidOperationException("Order item does not exist.");

            var product = await _productRepository.GetByIdAsync(productId);

            if (product != null)
            {
                product.StockQuantity += orderItem.Quantity;
                await _productRepository.UpdateAsync(product);
            }

            order.OrderItems.Remove(orderItem);

            await _orderRepository.UpdateOrderAsync(order);
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateOrderAsync(Order updatedOrder)
    {
        if (updatedOrder == null || updatedOrder.OrderItems.Count == 0)
            throw new ArgumentException("Updated order is invalid or empty.");

        using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var existingOrder = await _orderRepository.GetByIdAsync(updatedOrder.Id) ??
                throw new InvalidOperationException("Order does not exist.");

            await ProcessAddedItems(existingOrder, updatedOrder);
            await ProcessRemovedItems(existingOrder, updatedOrder);
            await ProcessUpdatedItems(existingOrder, updatedOrder);

            existingOrder.UpdateItems(updatedOrder.OrderItems);
            await _orderRepository.UpdateOrderAsync(existingOrder);

            await transaction.CommitAsync();
        }
        catch (ConcurrencyConflictException ex)
        {
            await transaction.RollbackAsync();
            throw new("Order was updated by another user.", ex);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task ProcessAddedItems(Order existingOrder, Order updatedOrder)
    {
        var addedItems = updatedOrder.OrderItems.Where(ui => !existingOrder.OrderItems.Any(ei => ei.ProductId == ui.ProductId)).ToList();

        foreach (var item in addedItems)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);

            if (product == null || item.Quantity > product.StockQuantity)
                throw new InvalidOperationException("Product is unavailable or stock is insufficient.");

            product.StockQuantity -= item.Quantity;
            await _productRepository.UpdateAsync(product);
        }
    }

    private async Task ProcessRemovedItems(Order existingOrder, Order updatedOrder)
    {
        var removedItems = existingOrder.OrderItems.Where(ei => !updatedOrder.OrderItems.Any(ui => ui.ProductId == ei.ProductId)).ToList();

        foreach (var item in removedItems)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);

            if (product != null)
            {
                product.StockQuantity += item.Quantity;
                await _productRepository.UpdateAsync(product);
            }
        }
    }

    private async Task ProcessUpdatedItems(Order existingOrder, Order updatedOrder)
    {
        var updatedItems = updatedOrder.OrderItems.Where(ui => existingOrder.OrderItems.Any(ei => ei.ProductId == ui.ProductId && ei.Quantity != ui.Quantity)).ToList();

        foreach (var item in updatedItems)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId) ??
                throw new InvalidOperationException("Product does not exist.");

            var existingItem = existingOrder.OrderItems.First(ei => ei.ProductId == item.ProductId);
            var quantityDifference = item.Quantity - existingItem.Quantity;

            if (quantityDifference > 0 && quantityDifference > product.StockQuantity)
                throw new InvalidOperationException("Insufficient stock for product.");

            product.StockQuantity -= quantityDifference;
            await _productRepository.UpdateAsync(product);
        }
    }

    //public async Task<bool> ProcessPaymentAsync(Order order)
    //{
    //    // Integrate with a payment gateway
    //    var paymentService = new PaymentService();
    //    return await paymentService.ProcessPayment(order.User, order.TotalPrice);
    //}
}