using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MarketNest.Domain.Enums;

namespace MarketNest.Infrastructure.BackgroundJobs;

public class BookingReminderJob : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BookingReminderJob> _logger;
    private Timer? _timer;

    public BookingReminderJob(
        IServiceScopeFactory scopeFactory,
        ILogger<BookingReminderJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Booking Reminder Job starting");
        _timer = new Timer(RunJob, null, TimeSpan.Zero, TimeSpan.FromHours(1));
        return Task.CompletedTask;
    }

    private async void RunJob(object? state)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<Persistence.AppDbContext>();

            var reminderHours = 24;
            var threshold = DateTime.UtcNow.AddHours(reminderHours);

            var bookings = await context.Bookings
                .Include(b => b.Buyer)
                .Include(b => b.Slot)
                .Where(b => b.Status == BookingStatus.Confirmed &&
                            b.Slot.StartTime <= threshold &&
                            b.Slot.StartTime > DateTime.UtcNow)
                .ToListAsync();

            foreach (var booking in bookings)
            {
                // Send notification logic here
                _logger.LogInformation($"Sending reminder for booking {booking.BookingNumber}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Booking Reminder Job");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Booking Reminder Job stopping");
        _timer?.Change(Timeout.Infinite, Timeout.Infinite);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
