using MarketNest.Domain.Entities;
using MarketNest.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

namespace MarketNest.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        if (context.Database.IsRelational())
        {
            await context.Database.MigrateAsync();
        }

        if (!context.Categories.Any())
        {
            await context.Categories.AddRangeAsync(CategorySeed.GetDefaultCategories());
            await context.SaveChangesAsync();
        }

        if (!context.PlatformSettings.Any())
        {
            await context.PlatformSettings.AddRangeAsync(PlatformSettingsSeed.GetDefaultSettings());
            await context.SaveChangesAsync();
        }
    }
}
