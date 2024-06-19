using MarketNest.Domain.Common;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Enums;

namespace MarketNest.Domain.Interfaces.Repositories;

public interface IListingRepository : IGenericRepository<Listing>
{
    Task<PagedResult<Listing>> SearchAsync(
        string? searchText,
        Guid? categoryId,
        int? priceMinCents,
        int? priceMaxCents,
        decimal? minRating,
        bool? isVirtual,
        string? city,
        ListingSortBy sortBy,
        int page,
        int pageSize);
    Task<PagedResult<Listing>> GetByVendorAsync(Guid vendorId, int page, int pageSize);
}

public enum ListingSortBy
{
    Relevance,
    PriceAsc,
    PriceDesc,
    Rating,
    Newest
}
