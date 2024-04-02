using LEDLuxe.Core.Entities.Products;
using LEDLuxe.Core.Interfaces.Repositories;
using LEDLuxe.Core.Interfaces.Services;

namespace LEDLuxe.Core.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product> GetProductByIdAsync(Guid productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product == null || product.IsDeleted)
            throw new InvalidOperationException($"Product with ID {productId} not found or has been deleted.");

        return product;
    }

    public async Task AddProductAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product, nameof(product));

        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Product name cannot be empty.", nameof(product));

        await _productRepository.AddAsync(product);
    }

    public async Task UpdateProductAsync(Product updatedProduct)
    {
        ArgumentNullException.ThrowIfNull(updatedProduct, nameof(updatedProduct));

        var product = await _productRepository.GetByIdAsync(updatedProduct.Id) ??
            throw new InvalidOperationException($"Product with ID {updatedProduct.Id} not found or has been deleted.");

        if (updatedProduct.StockQuantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative.", nameof(updatedProduct));

        product.Name = updatedProduct.Name;
        product.Description = updatedProduct.Description;
        product.Price = updatedProduct.Price;
        product.StockQuantity = updatedProduct.StockQuantity;

        await _productRepository.UpdateAsync(product);
    }

    public async Task DeleteProductAsync(Guid productId)
    {
        var product = await _productRepository.GetByIdAsync(productId) ??
            throw new InvalidOperationException($"Product with ID {productId} not found.");

        product.IsDeleted = true;
        await _productRepository.UpdateAsync(product);
    }

    public async Task AddRateAsync(Guid productId, Rate rate)
    {
        ArgumentNullException.ThrowIfNull(rate, nameof(rate));

        if (rate.Value < 0 || rate.Value > 50)
            throw new ArgumentException("Rate value must be between 0 and 50.", nameof(rate));

        var product = await _productRepository.GetByIdAsync(productId) ??
                      throw new InvalidOperationException($"Product with ID {productId} not found or has been deleted.");

        if (product.Rates.Any(r => r.UserId == rate.UserId))
            throw new InvalidOperationException("User has already rated this product.");

        product.AddRate(rate);
        await _productRepository.UpdateAsync(product);
    }

    public async Task RemoveRateAsync(Guid productId, Guid rateId)
    {
        var product = await _productRepository.GetByIdAsync(productId) ??
                      throw new InvalidOperationException($"Product with ID {productId} not found or has been deleted.");

        var rate = product.Rates.FirstOrDefault(r => r.Id == rateId) ??
            throw new InvalidOperationException($"Rate with ID {rateId} not found for the product.");

        product.RemoveRate(rate);
        await _productRepository.UpdateAsync(product);
    }

    public async Task<double> CalculateAverageRatingAsync(Guid productId)
    {
        var product = await _productRepository.GetByIdAsync(productId) ??
                      throw new InvalidOperationException($"Product with ID {productId} not found or has been deleted.");

        return product.CalculateAverageRating();
    }
}