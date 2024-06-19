using MarketNest.Domain.Common;
using MarketNest.Domain.Entities;

namespace MarketNest.Domain.Interfaces.Repositories;

public interface IBookingRepository : IGenericRepository<Booking>
{
    Task<PagedResult<Booking>> GetByBuyerAsync(Guid buyerId, int page, int pageSize);
    Task<PagedResult<Booking>> GetByVendorAsync(Guid vendorId, int page, int pageSize);
    Task<Booking?> GetBySlotIdAsync(Guid slotId);
}
