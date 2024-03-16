using LEDLuxe.Core.Entities.Products;

namespace LEDLuxe.Core.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<Category> GetByIdAsync(Guid id);

    Task<List<Category>> GetAllAsync();

    Task AddAsync(Category category);

    Task UpdateAsync(Category category);

    Task DeleteAsync(Category category);
}