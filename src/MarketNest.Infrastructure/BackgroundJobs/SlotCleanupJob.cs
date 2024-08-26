using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MarketNest.Domain.Enums;

namespace MarketNest.Infrastructure.BackgroundJobs;

public class SlotCleanupJob : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SlotCleanupJob> _logger;
    private Timer? _timer;

    public SlotCleanupJob(
        IServiceScopeFactory scopeFactory,
        ILogger<SlotCleanupJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Slot Cleanup Job starting");
        _timer = new Timer(RunJob, null, TimeSpan.Zero, TimeSpan.FromDays(1));
        return Task.CompletedTask;
    }

    private async void RunJob(object? state)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<Persistence.AppDbContext>();

            var threshold = DateTime.UtcNow.AddDays(-7);

            var oldSlots = await context.AvailabilitySlots
                .Where(s => s.EndTime < threshold && !s.IsBooked)
                .ToListAsync();

            if (oldSlots.Any())
            {
                context.AvailabilitySlots.RemoveRange(oldSlots);
                await context.SaveChangesAsync();
                _logger.LogInformation($"Cleaned up {oldSlots.Count} old availability slots");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Slot Cleanup Job");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Slot Cleanup Job stopping");
        _timer?.Change(Timeout.Infinite, Timeout.Infinite);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
