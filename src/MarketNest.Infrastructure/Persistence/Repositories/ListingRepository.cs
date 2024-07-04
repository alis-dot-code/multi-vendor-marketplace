using MarketNest.Domain.Common;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Enums;
using MarketNest.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MarketNest.Infrastructure.Persistence.Repositories;

public class ListingRepository : GenericRepository<Listing>, IListingRepository
{
    public ListingRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PagedResult<Listing>> SearchAsync(
        string? searchText,
        Guid? categoryId,
        int? priceMinCents,
        int? priceMaxCents,
        decimal? minRating,
        bool? isVirtual,
        string? city,
        ListingSortBy sortBy,
        int page,
        int pageSize)
    {
        var query = _dbSet
            .Include(l => l.Images)
            .Include(l => l.Vendor)
            .Include(l => l.Category)
            .AsNoTracking()
            .Where(l => l.Status == ListingStatus.Active)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(l => EF.Functions.ToTsVector("english", l.Title + " " + l.Description)
                .Matches(EF.Functions.PlainToTsQuery("english", searchText)));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(l => l.CategoryId == categoryId);
        }

        if (priceMinCents.HasValue)
        {
            query = query.Where(l => l.PriceCents >= priceMinCents.Value);
        }

        if (priceMaxCents.HasValue)
        {
            query = query.Where(l => l.PriceCents <= priceMaxCents.Value);
        }

        if (minRating.HasValue)
        {
            query = query.Where(l => l.AvgRating >= minRating.Value);
        }

        if (isVirtual.HasValue)
        {
            query = query.Where(l => l.IsVirtual == isVirtual.Value);
        }

        if (!string.IsNullOrWhiteSpace(city))
        {
            query = query.Where(l => l.Vendor.City != null && l.Vendor.City.ToLower().Contains(city.ToLower()));
        }

        query = sortBy switch
        {
            ListingSortBy.PriceAsc => query.OrderBy(l => l.PriceCents),
            ListingSortBy.PriceDesc => query.OrderByDescending(l => l.PriceCents),
            ListingSortBy.Rating => query.OrderByDescending(l => l.AvgRating),
            ListingSortBy.Newest => query.OrderByDescending(l => l.CreatedAt),
            ListingSortBy.Relevance or _ => searchText != null
                ? query.OrderByDescending(l => EF.Functions.ToTsVector("english", l.Title + " " + l.Description)
                    .Matches(EF.Functions.PlainToTsQuery("english", searchText)))
                : query.OrderByDescending(l => l.CreatedAt)
        };

        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<Listing>(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<Listing>> GetByVendorAsync(Guid vendorId, int page, int pageSize)
    {
        var query = _dbSet
            .Include(l => l.Images)
            .AsNoTracking()
            .Where(l => l.VendorId == vendorId);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(l => l.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Listing>(items, totalCount, page, pageSize);
    }
}
