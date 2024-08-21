using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Interfaces;

namespace MarketNest.Infrastructure.Services;

public class CalendarExportService : ICalendarExportService
{
    public Task<string> GenerateICalFeed(List<BookingDto> bookings)
    {
        var calendar = new Calendar();

        foreach (var booking in bookings)
        {
            var evt = new CalendarEvent
            {
                Summary = booking.ListingTitle,
                Description = $"Booking #{booking.BookingNumber}\\nAttendees: {booking.Attendees}",
                Start = new CalDateTime(booking.SlotStart),
                End = new CalDateTime(booking.SlotEnd),
                Uid = booking.Id.ToString()
            };

            calendar.Events.Add(evt);
        }

        var serializer = new Ical.Net.Serialization.CalendarSerializer();
        return Task.FromResult(serializer.SerializeToString(calendar));
    }
}
