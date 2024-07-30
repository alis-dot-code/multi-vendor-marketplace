using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Interfaces.Repositories;
using AutoMapper;

namespace MarketNest.Application.Listings;

public record GetListingByIdQuery(Guid Id) : IRequest<ListingDetailDto>;

public class GetListingByIdHandler : IRequestHandler<GetListingByIdQuery, ListingDetailDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetListingByIdHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ListingDetailDto> Handle(GetListingByIdQuery request, CancellationToken cancellationToken)
    {
        var listing = await _uow.Listings.GetByIdAsync(request.Id) ?? throw new NotFoundException("Listing not found");
        return _mapper.Map<ListingDetailDto>(listing);
    }
}
