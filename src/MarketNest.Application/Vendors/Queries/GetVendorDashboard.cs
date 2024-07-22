using MediatR;
using AutoMapper;
using MarketNest.Application.Common.DTOs;
using MarketNest.Domain.Enums;
using MarketNest.Domain.Interfaces.Repositories;

namespace MarketNest.Application.Vendors.Queries;

public record GetVendorDashboardQuery(Guid VendorId) : IRequest<VendorDashboardDto>;

public class GetVendorDashboardHandler : IRequestHandler<GetVendorDashboardQuery, VendorDashboardDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetVendorDashboardHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<VendorDashboardDto> Handle(GetVendorDashboardQuery request, CancellationToken cancellationToken)
    {
        var vendor = await _uow.Vendors.GetByIdAsync(request.VendorId);
        if (vendor == null) throw new MarketNest.Application.Common.Exceptions.NotFoundException("Vendor not found");

        var bookings = (await _uow.Bookings.FindAsync(b => b.VendorId == vendor.Id)).ToList();
        var totalEarnings = bookings.Sum(b => b.VendorAmountCents);
        var thisMonth = bookings.Where(b => b.CreatedAt.Month == DateTime.UtcNow.Month && b.CreatedAt.Year == DateTime.UtcNow.Year).Sum(b => b.VendorAmountCents);
        var totalBookings = bookings.Count;
        var pending = bookings.Count(b => b.Status == BookingStatus.Pending);
        var recent = bookings.OrderByDescending(b => b.CreatedAt).Take(10).Select(b => new BookingDto {
            Id = b.Id,
            BookingNumber = b.BookingNumber,
            Status = b.Status.ToString(),
            ListingTitle = b.Listing?.Title ?? string.Empty,
            SlotStart = b.Slot?.StartTime ?? DateTime.MinValue,
            SlotEnd = b.Slot?.EndTime ?? DateTime.MinValue,
            TotalCents = b.TotalCents,
            PlatformFeeCents = b.PlatformFeeCents,
            Attendees = b.Attendees
        }).ToList();

        return new VendorDashboardDto
        {
            TotalEarningsCents = totalEarnings,
            ThisMonthEarningsCents = thisMonth,
            TotalBookings = totalBookings,
            PendingBookings = pending,
            AvgRating = vendor.AvgRating,
            RecentBookings = recent
        };
    }
}
