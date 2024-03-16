using LEDLuxe.Core.Entities.Products;
using System.Text.Json.Serialization;

namespace LEDLuxe.Core.Entities.Orders;

public class Order
{
    public Guid Id { get; private set; }

    public DateTime OrderDate { get; private set; }

    [JsonIgnore]
    public decimal TotalPrice => OrderItems.Sum(item => item.UnitPrice * item.Quantity);

    public Guid UserId { get; private set; }

    public ICollection<OrderItem> OrderItems { get; private set; } = [];

    public Order(Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        OrderDate = DateTime.UtcNow;
        OrderItems = [];
    }

    public void AddItem(Product product, int quantity)
    {
        var item = OrderItems.FirstOrDefault(i => i.ProductId == product.Id);

        if (item == null)
        {
            item = new OrderItem(Id, product.Id, product.Price, quantity);
            OrderItems.Add(item);
        }
        else
            item.IncreaseQuantity(quantity);
    }

    public void RemoveItem(Guid productId)
    {
        var item = OrderItems.FirstOrDefault(i => i.ProductId == productId);

        if (item != null)
            OrderItems.Remove(item);
    }

    public void UpdateItemQuantity(Guid productId, int quantity)
    {
        var item = OrderItems.FirstOrDefault(i => i.ProductId == productId);

        if (item != null)
        {
            if (quantity <= 0)
                OrderItems.Remove(item);

            else
                item.UpdateQuantity(quantity);
        }
    }
}