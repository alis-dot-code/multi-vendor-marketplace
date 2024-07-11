using MarketNest.Application.Common.DTOs;

namespace MarketNest.Application.Common.Interfaces;

public interface ICalendarExportService
{
    Task<string> GenerateICalFeed(List<BookingDto> bookings);
}
