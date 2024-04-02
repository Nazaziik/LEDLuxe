using LEDLuxe.Core.Entities.Products;

namespace LEDLuxe.Core.Interfaces.Services;

public interface IProductService
{
    Task AddProductAsync(Product product);

    Task AddRateAsync(Guid productId, Rate rate);

    Task<double> CalculateAverageRatingAsync(Guid productId);

    Task DeleteProductAsync(Guid productId);

    Task<IEnumerable<Product>> GetAllProductsAsync();

    Task<Product> GetProductByIdAsync(Guid productId);

    Task RemoveRateAsync(Guid productId, Guid rateId);

    Task UpdateProductAsync(Product updatedProduct);
}