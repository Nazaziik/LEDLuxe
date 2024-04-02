using LEDLuxe.Core.Entities.Products;

namespace LEDLuxe.Core.Interfaces.Repositories;

public interface IProductRepository
{
    Task<Product> GetByIdAsync(Guid id);

    Task<IEnumerable<Product>> GetAllAsync(bool includeDeleted = false);

    Task AddAsync(Product product);

    Task UpdateAsync(Product product);

    Task DeleteAsync(Guid productId);

    Task AddPhotoUrlAsync(Guid productId, string url);

    Task RemovePhotoUrlAsync(Guid productId, string url);

    Task UpdatePhotoUrlAsync(Guid productId, string oldUrl, string newUrl);

    Task AddRateAsync(Guid productId, Rate rate);

    Task RemoveRateAsync(Guid productId, Rate rate);

    Task<IEnumerable<Rate>> GetRatesByProductIdAsync(Guid productId);

    Task AddCategoryAsync(Guid productId, Category category);

    Task RemoveCategoryAsync(Guid productId, Category category);

    Task<IEnumerable<Category>> GetCategoriesByProductIdAsync(Guid productId);
}