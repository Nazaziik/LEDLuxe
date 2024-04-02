namespace LEDLuxe.Core.Entities.Products;

public class Rate
{
    public Guid Id { get; private set; }

    public int Value { get; private set; }

    public string Comment { get; private set; }

    public Guid UserId { get; private set; }

    public Guid ProductId { get; private set; }

    public Product Product { get; private set; }

    public Rate(Guid productId, Guid userId, int value, string comment = "")
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        UserId = userId;
        SetValue(value);
        Comment = comment;
    }

    private void SetValue(int value)
    {
        if (value < 1 || value > 100)
            throw new ArgumentException("Value must be between 1 and 100.");

        Value = value;
    }

    public void SetComment(string comment)
    {
        Comment = comment;
    }
}