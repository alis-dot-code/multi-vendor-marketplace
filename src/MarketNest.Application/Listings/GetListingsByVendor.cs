using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Interfaces.Repositories;
using AutoMapper;
using MarketNest.Domain.Common;

namespace MarketNest.Application.Listings;

public record GetListingsByVendorQuery(Guid VendorId, int Page, int PageSize) : IRequest<PagedResult<ListingDto>>;

public class GetListingsByVendorHandler : IRequestHandler<GetListingsByVendorQuery, PagedResult<ListingDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetListingsByVendorHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PagedResult<ListingDto>> Handle(GetListingsByVendorQuery request, CancellationToken cancellationToken)
    {
        var page = request.Page <= 0 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;
        var result = await _uow.Listings.GetByVendorAsync(request.VendorId, page, pageSize);
        var mapped = result.Items.Select(l => _mapper.Map<ListingDto>(l)).ToList();
        return new PagedResult<ListingDto>(mapped, result.TotalCount, result.Page, result.PageSize);
    }
}
