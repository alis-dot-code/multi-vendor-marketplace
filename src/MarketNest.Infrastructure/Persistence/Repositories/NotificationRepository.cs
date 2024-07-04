using MarketNest.Domain.Common;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MarketNest.Infrastructure.Persistence.Repositories;

public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
    public NotificationRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PagedResult<Notification>> GetByUserAsync(Guid userId, int page, int pageSize)
    {
        var query = _dbSet
            .AsNoTracking()
            .Where(n => n.UserId == userId);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Notification>(items, totalCount, page, pageSize);
    }

    public async Task<int> GetUnreadCountAsync(Guid userId)
    {
        return await _dbSet.CountAsync(n => n.UserId == userId && !n.IsRead);
    }
}
