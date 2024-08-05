using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Domain.Common;
using AutoMapper;
using MarketNest.Domain.Interfaces.Repositories;

namespace MarketNest.Application.Bookings.Queries;

public record GetVendorBookingsQuery(Guid VendorId, int Page, int PageSize) : IRequest<PagedResult<BookingDto>>;

public class GetVendorBookingsHandler : IRequestHandler<GetVendorBookingsQuery, PagedResult<BookingDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetVendorBookingsHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PagedResult<BookingDto>> Handle(GetVendorBookingsQuery request, CancellationToken cancellationToken)
    {
        var page = request.Page <= 0 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

        var all = (await _uow.Bookings.FindAsync(b => b.VendorId == request.VendorId)).ToList();
        var total = all.Count;
        var items = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var mapped = items.Select(i => _mapper.Map<BookingDto>(i)).ToList();
        return new PagedResult<BookingDto>(mapped, total, page, pageSize);
    }
}
