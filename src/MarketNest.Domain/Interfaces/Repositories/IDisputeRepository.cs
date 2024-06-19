using MarketNest.Domain.Common;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Enums;

namespace MarketNest.Domain.Interfaces.Repositories;

public interface IDisputeRepository : IGenericRepository<Dispute>
{
    Task<PagedResult<Dispute>> GetByStatusAsync(DisputeStatus status, int page, int pageSize);
}
