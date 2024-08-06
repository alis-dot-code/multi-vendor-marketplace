using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Domain.Common;
using AutoMapper;
using MarketNest.Domain.Interfaces.Repositories;

namespace MarketNest.Application.Payments.Queries;

public record GetVendorPaymentHistoryQuery(Guid VendorId, int Page, int PageSize) : IRequest<PagedResult<PaymentDto>>;

public class GetVendorPaymentHistoryHandler : IRequestHandler<GetVendorPaymentHistoryQuery, PagedResult<PaymentDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetVendorPaymentHistoryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PagedResult<PaymentDto>> Handle(GetVendorPaymentHistoryQuery request, CancellationToken cancellationToken)
    {
        var page = request.Page <= 0 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

        var all = (await _uow.Payments.FindAsync(p => p.Booking.VendorId == request.VendorId)).ToList();
        var total = all.Count;
        var items = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var mapped = items.Select(i => _mapper.Map<PaymentDto>(i)).ToList();
        return new PagedResult<PaymentDto>(mapped, total, page, pageSize);
    }
}
