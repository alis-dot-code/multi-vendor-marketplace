using MarketNest.Domain.Common;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MarketNest.Infrastructure.Persistence.Repositories;

public class ReviewRepository : GenericRepository<Review>, IReviewRepository
{
    public ReviewRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PagedResult<Review>> GetByListingAsync(Guid listingId, int page, int pageSize)
    {
        var query = _dbSet
            .Include(r => r.Buyer)
            .Include(r => r.Reply)
            .AsNoTracking()
            .Where(r => r.ListingId == listingId && r.IsVisible);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Review>(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<Review>> GetByVendorAsync(Guid vendorId, int page, int pageSize)
    {
        var query = _dbSet
            .Include(r => r.Buyer)
            .Include(r => r.Reply)
            .AsNoTracking()
            .Where(r => r.VendorId == vendorId && r.IsVisible);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Review>(items, totalCount, page, pageSize);
    }

    public async Task<bool> ExistsByBookingAsync(Guid bookingId)
    {
        return await _dbSet.AnyAsync(r => r.BookingId == bookingId);
    }
}
