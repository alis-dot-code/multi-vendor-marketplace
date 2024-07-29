using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Interfaces.Repositories;
using AutoMapper;
using MarketNest.Domain.Common;

namespace MarketNest.Application.Listings;

public record SearchListingsQuery(string? Q, Guid? CategoryId, int? PriceMinCents, int? PriceMaxCents, decimal? MinRating, bool? IsVirtual, string? City, Domain.Interfaces.Repositories.ListingSortBy SortBy, int Page, int PageSize) : IRequest<PagedResult<ListingDto>>;

public class SearchListingsHandler : IRequestHandler<SearchListingsQuery, PagedResult<ListingDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public SearchListingsHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PagedResult<ListingDto>> Handle(SearchListingsQuery request, CancellationToken cancellationToken)
    {
        var page = request.Page <= 0 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;
        var sort = request.SortBy;
        var result = await _uow.Listings.SearchAsync(request.Q, request.CategoryId, request.PriceMinCents, request.PriceMaxCents, request.MinRating, request.IsVirtual, request.City, sort, page, pageSize);
        var mapped = result.Items.Select(l => _mapper.Map<ListingDto>(l)).ToList();
        return new PagedResult<ListingDto>(mapped, result.TotalCount, result.Page, result.PageSize);
    }
}
