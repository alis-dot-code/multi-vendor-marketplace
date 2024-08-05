using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Domain.Interfaces.Repositories;

namespace MarketNest.Application.Bookings;

public record ExportCalendarQuery(Guid VendorId, DateTime From, DateTime To) : IRequest<string>;

public class ExportCalendarHandler : IRequestHandler<ExportCalendarQuery, string>
{
    private readonly IUnitOfWork _uow;
    private readonly ICalendarExportService _calendar;

    public ExportCalendarHandler(IUnitOfWork uow, ICalendarExportService calendar)
    {
        _uow = uow;
        _calendar = calendar;
    }

    public async Task<string> Handle(ExportCalendarQuery request, CancellationToken cancellationToken)
    {
        var bookings = (await _uow.Bookings.FindAsync(b => b.VendorId == request.VendorId && b.Status == MarketNest.Domain.Enums.BookingStatus.Confirmed)).ToList();
        var mapped = bookings.Select(b => new BookingDto
        {
            Id = b.Id,
            BookingNumber = b.BookingNumber,
            Status = b.Status.ToString(),
            ListingTitle = b.Listing.Title,
            SlotStart = b.Slot.StartTime,
            SlotEnd = b.Slot.EndTime,
            TotalCents = b.TotalCents,
            PlatformFeeCents = b.PlatformFeeCents,
            Attendees = b.Attendees
        }).ToList();

        var ics = await _calendar.GenerateICalFeed(mapped);
        return ics;
    }
}
