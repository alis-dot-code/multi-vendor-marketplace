using MarketNest.Domain.Common;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MarketNest.Infrastructure.Persistence.Repositories;

public class BookingRepository : GenericRepository<Booking>, IBookingRepository
{
    public BookingRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PagedResult<Booking>> GetByBuyerAsync(Guid buyerId, int page, int pageSize)
    {
        var query = _dbSet
            .Include(b => b.Listing)
            .Include(b => b.Vendor)
            .Include(b => b.Slot)
            .AsNoTracking()
            .Where(b => b.BuyerId == buyerId);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(b => b.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Booking>(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<Booking>> GetByVendorAsync(Guid vendorId, int page, int pageSize)
    {
        var query = _dbSet
            .Include(b => b.Listing)
            .Include(b => b.Buyer)
            .Include(b => b.Slot)
            .AsNoTracking()
            .Where(b => b.VendorId == vendorId);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(b => b.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Booking>(items, totalCount, page, pageSize);
    }

    public async Task<Booking?> GetBySlotIdAsync(Guid slotId)
    {
        return await _dbSet
            .Include(b => b.Listing)
            .Include(b => b.Buyer)
            .Include(b => b.Vendor)
            .FirstOrDefaultAsync(b => b.SlotId == slotId);
    }
}
