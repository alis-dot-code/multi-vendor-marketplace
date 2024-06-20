using MarketNest.Domain.Common;
using MarketNest.Domain.Entities;

namespace MarketNest.Domain.Interfaces.Repositories;

public interface INotificationRepository : IGenericRepository<Notification>
{
    Task<PagedResult<Notification>> GetByUserAsync(Guid userId, int page, int pageSize);
    Task<int> GetUnreadCountAsync(Guid userId);
}
