using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using MarketNest.Infrastructure.Persistence;
using MarketNest.Infrastructure.Persistence.Repositories;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Interfaces.Repositories;
using MarketNest.Infrastructure.Services;

namespace MarketNest.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Redis
        services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!));

        // Repositories + UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IStripeService, StripeService>();
        services.AddScoped<ICloudinaryService, CloudinaryService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<ICalendarExportService, CalendarExportService>();

        // Background Jobs
        services.AddHostedService<BackgroundJobs.BookingReminderJob>();
        services.AddHostedService<BackgroundJobs.SlotCleanupJob>();

        return services;
    }
}
