using MarketNest.Domain.Common;
using MarketNest.Domain.Entities;

namespace MarketNest.Domain.Interfaces.Repositories;

public interface IReviewRepository : IGenericRepository<Review>
{
    Task<PagedResult<Review>> GetByListingAsync(Guid listingId, int page, int pageSize);
    Task<PagedResult<Review>> GetByVendorAsync(Guid vendorId, int page, int pageSize);
    Task<bool> ExistsByBookingAsync(Guid bookingId);
}
