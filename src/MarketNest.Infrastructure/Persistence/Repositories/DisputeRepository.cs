using MarketNest.Domain.Common;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Enums;
using MarketNest.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MarketNest.Infrastructure.Persistence.Repositories;

public class DisputeRepository : GenericRepository<Dispute>, IDisputeRepository
{
    public DisputeRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PagedResult<Dispute>> GetByStatusAsync(DisputeStatus status, int page, int pageSize)
    {
        var query = _dbSet
            .Include(d => d.Booking)
            .Include(d => d.Opener)
            .AsNoTracking()
            .Where(d => d.Status == status);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(d => d.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Dispute>(items, totalCount, page, pageSize);
    }
}
