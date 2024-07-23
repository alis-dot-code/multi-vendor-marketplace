using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Domain.Interfaces.Repositories;
using AutoMapper;

namespace MarketNest.Application.Vendors.Queries;

public record GetPendingVendorsQuery() : IRequest<List<VendorDto>>;

public class GetPendingVendorsHandler : IRequestHandler<GetPendingVendorsQuery, List<VendorDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetPendingVendorsHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<List<VendorDto>> Handle(GetPendingVendorsQuery request, CancellationToken cancellationToken)
    {
        var list = await _uow.Vendors.GetPendingAsync();
        return list.Select(v => _mapper.Map<VendorDto>(v)).ToList();
    }
}
