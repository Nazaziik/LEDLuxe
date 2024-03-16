using LEDLuxe.Core.Entities.Products;
using LEDLuxe.Core.Entities.Rates;
using LEDLuxe.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LEDLuxe.Infrastructure.Repositories;

public class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddAsync(Product category)
    {
        _context.Products.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product category)
    {
        _context.Products.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> GetByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task UpdateAsync(Product category)
    {
        _context.Entry(category).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task AddCategoryAsync(Product product, Category category)
    {
        product.AddCategory(category);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveCategoryAsync(Product product, Category category)
    {
        product.RemoveCategory(category);
        await _context.SaveChangesAsync();
    }

    public async Task AddRateAsync(Product product, Rate rate)
    {
        product.AddRate(rate);
        await _context.SaveChangesAsync();
    }
}