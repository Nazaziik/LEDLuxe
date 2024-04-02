using LEDLuxe.Core.Entities.Products;
using LEDLuxe.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LEDLuxe.Infrastructure.Repositories;

public class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid productId)
    {
        var product = await _context.Products.FindAsync(productId);

        if (product != null)
        {
            product.IsDeleted = true;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Product> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .Include(p => p.Rates)
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(bool includeDeleted = false)
    {
        IQueryable<Product> query = _context.Products.AsQueryable();

        if (!includeDeleted)
            query = query.Where(p => !p.IsDeleted);

        return await query.Include(p => p.Rates)
                          .Include(p => p.Categories)
                          .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Rates)
            .Include(p => p.Categories)
            .ToListAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task AddPhotoUrlAsync(Guid productId, string url)
    {
        var product = await GetByIdAsync(productId);

        if (product != null)
        {
            product.AddPhotoUrl(url);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemovePhotoUrlAsync(Guid productId, string url)
    {
        var product = await GetByIdAsync(productId);

        if (product != null)
        {
            product.RemovePhotoUrl(url);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdatePhotoUrlAsync(Guid productId, string oldUrl, string newUrl)
    {
        var product = await GetByIdAsync(productId);

        if (product != null)
        {
            product.UpdatePhotoUrl(oldUrl, newUrl);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddRateAsync(Guid productId, Rate rate)
    {
        var product = await GetByIdAsync(productId);

        if (product != null)
        {
            product.AddRate(rate);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveRateAsync(Guid productId, Rate rate)
    {
        var rateToRemove = await _context.Rates.FindAsync(rate.Id);

        if (rateToRemove != null)
        {
            _context.Rates.Remove(rateToRemove);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Rate>> GetRatesByProductIdAsync(Guid productId)
    {
        return await _context.Rates
            .Where(r => r.ProductId == productId)
            .ToListAsync();
    }

    public async Task AddCategoryAsync(Guid productId, Category category)
    {
        var product = await GetByIdAsync(productId);

        if (product != null && !product.Categories.Contains(category))
        {
            product.AddCategory(category);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveCategoryAsync(Guid productId, Category category)
    {
        var product = await GetByIdAsync(productId);

        if (product != null && product.Categories.Contains(category))
        {
            product.RemoveCategory(category);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Category>> GetCategoriesByProductIdAsync(Guid productId)
    {
        var product = await GetByIdAsync(productId);
        return product?.Categories ?? [];
    }
}