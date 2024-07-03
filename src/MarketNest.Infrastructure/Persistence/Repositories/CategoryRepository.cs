using MarketNest.Domain.Entities;
using MarketNest.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MarketNest.Infrastructure.Persistence.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Category>> GetTreeAsync()
    {
        var categories = await _dbSet
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ToListAsync();

        var parentCategories = categories.Where(c => !c.ParentId.HasValue).ToList();
        
        foreach (var parent in parentCategories)
        {
            parent.Children = categories.Where(c => c.ParentId == parent.Id).ToList();
        }

        return parentCategories;
    }

    public async Task<Category?> GetBySlugAsync(string slug)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Slug == slug && c.IsActive);
    }
}
