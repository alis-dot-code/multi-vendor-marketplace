using MediatR;
using AutoMapper;
using MarketNest.Application.Common.DTOs;
using MarketNest.Domain.Interfaces.Repositories;
using MarketNest.Application.Common.Interfaces;

namespace MarketNest.Application.Vendors.Queries;

public record GetVendorBySlugQuery(string Slug) : IRequest<VendorDto>;

public class GetVendorBySlugHandler : IRequestHandler<GetVendorBySlugQuery, VendorDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetVendorBySlugHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<VendorDto> Handle(GetVendorBySlugQuery request, CancellationToken cancellationToken)
    {
        var vendor = await _uow.Vendors.FindBySlugAsync(request.Slug);
        if (vendor == null) throw new MarketNest.Application.Common.Exceptions.NotFoundException("Vendor not found");

        // get active listings (first page 1 with large pageSize)
        var listings = await _uow.Listings.GetByVendorAsync(vendor.Id, 1, 50);

        var dto = _mapper.Map<VendorDto>(vendor);
        // attach limited listing info if mapping exists; otherwise ignore
        return dto;
    }
}
