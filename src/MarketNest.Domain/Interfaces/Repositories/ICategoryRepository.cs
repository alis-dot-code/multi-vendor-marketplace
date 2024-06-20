using MarketNest.Domain.Entities;

namespace MarketNest.Domain.Interfaces.Repositories;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<IEnumerable<Category>> GetTreeAsync();
    Task<Category?> GetBySlugAsync(string slug);
}
