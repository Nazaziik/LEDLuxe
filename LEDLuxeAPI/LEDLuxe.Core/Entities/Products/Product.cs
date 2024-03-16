using LEDLuxe.Core.Entities.Rates;

namespace LEDLuxe.Core.Entities.Products;

public class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public List<string> PhotoUrls { get; private set; } = [];

    public ICollection<Category> Categories { get; private set; } = [];

    public ICollection<Rate> Rates { get; private set; } = [];

    public Product(string name, decimal price)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        PhotoUrls = [];
    }

    public void AddPhotoUrl(string url)
    {
        if (!string.IsNullOrWhiteSpace(url))
            PhotoUrls.Add(url);
    }

    public void RemovePhotoUrl(string url)
    {
        PhotoUrls.Remove(url);
    }

    public void UpdatePhotoUrl(string oldUrl, string newUrl)
    {
        if (string.IsNullOrWhiteSpace(newUrl))
            throw new ArgumentException("New URL cannot be empty.", nameof(newUrl));

        var index = PhotoUrls.IndexOf(oldUrl);

        if (index != -1)
            PhotoUrls[index] = newUrl;

        else
            AddPhotoUrl(newUrl);
    }

    public void AddRate(Rate rate)
    {
        ArgumentNullException.ThrowIfNull(rate);

        Rates.Add(rate);
    }

    public void RemoveRate(Rate rate)
    {
        ArgumentNullException.ThrowIfNull(rate);

        Rates.Remove(rate);
    }

    public double CalculateAverageRating()
    {
        if (Rates.Count == 0)
            return 0;
        
        return Rates.Average(rate => rate.Value);
    }

    public void AddCategory(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        if (!Categories.Contains(category))
        {
            Categories.Add(category);
            category.Products.Add(this);
        }
    }

    public void RemoveCategory(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        if (!Categories.Contains(category))
        {
            return;
        }

        Categories.Remove(category);
        category.Products.Remove(this);
    }
}