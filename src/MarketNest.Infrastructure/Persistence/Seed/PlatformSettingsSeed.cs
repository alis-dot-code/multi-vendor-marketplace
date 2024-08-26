using MarketNest.Domain.Entities;

namespace MarketNest.Infrastructure.Persistence.Seed;

public static class PlatformSettingsSeed
{
    public static List<PlatformSetting> GetDefaultSettings() => new()
    {
        new PlatformSetting { Id = Guid.Parse("20000000-0000-0000-0000-000000000001"), Key = "platform_fee_percentage", Value = "10", Description = "Platform fee percentage charged on each booking" },
        new PlatformSetting { Id = Guid.Parse("20000000-0000-0000-0000-000000000002"), Key = "default_currency", Value = "USD", Description = "Default currency for all transactions" },
        new PlatformSetting { Id = Guid.Parse("20000000-0000-0000-0000-000000000003"), Key = "booking_reminder_hours", Value = "24", Description = "Hours before booking to send reminder notification" }
    };
}
