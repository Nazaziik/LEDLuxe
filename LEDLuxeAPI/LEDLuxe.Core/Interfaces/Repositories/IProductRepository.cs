using LEDLuxe.Core.Entities.Products;
using LEDLuxe.Core.Entities.Rates;

namespace LEDLuxe.Core.Interfaces.Repositories;

public interface IProductRepository
{
    Task<Product> GetByIdAsync(Guid id);

    Task<List<Product>> GetAllAsync();

    Task AddAsync(Product product);

    Task UpdateAsync(Product product);

    Task DeleteAsync(Product product);

    Task AddCategoryAsync(Product product, Category category);

    Task RemoveCategoryAsync(Product product, Category category);

    Task AddRateAsync(Product product, Rate rate);
}