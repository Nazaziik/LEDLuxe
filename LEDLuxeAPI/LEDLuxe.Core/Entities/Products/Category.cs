namespace LEDLuxe.Core.Entities.Products;

public class Category
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public ICollection<Product> Products { get; private set; }

    public Category(string name)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Products = [];
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name cannot be null or empty.", nameof(newName));
        
        Name = newName;
    }
}