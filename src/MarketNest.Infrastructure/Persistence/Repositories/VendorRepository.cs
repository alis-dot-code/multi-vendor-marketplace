using MarketNest.Domain.Entities;
using MarketNest.Domain.Enums;
using MarketNest.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MarketNest.Infrastructure.Persistence.Repositories;

public class VendorRepository : GenericRepository<Vendor>, IVendorRepository
{
    public VendorRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Vendor?> FindBySlugAsync(string slug)
    {
        return await _dbSet
            .Include(v => v.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Slug == slug);
    }

    public async Task<Vendor?> FindByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Include(v => v.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.UserId == userId);
    }

    public async Task<IEnumerable<Vendor>> GetPendingAsync()
    {
        return await _dbSet
            .Include(v => v.User)
            .AsNoTracking()
            .Where(v => v.Status == VendorStatus.Pending)
            .OrderBy(v => v.CreatedAt)
            .ToListAsync();
    }
}
