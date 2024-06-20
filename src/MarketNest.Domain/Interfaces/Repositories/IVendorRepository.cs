using MarketNest.Domain.Entities;

namespace MarketNest.Domain.Interfaces.Repositories;

public interface IVendorRepository : IGenericRepository<Vendor>
{
    Task<Vendor?> FindBySlugAsync(string slug);
    Task<Vendor?> FindByUserIdAsync(Guid userId);
    Task<IEnumerable<Vendor>> GetPendingAsync();
}
