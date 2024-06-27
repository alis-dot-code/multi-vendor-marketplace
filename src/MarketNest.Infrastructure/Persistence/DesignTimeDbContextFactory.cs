using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MarketNest.Infrastructure.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            var conn = Environment.GetEnvironmentVariable("DefaultConnection")
                       ?? "Host=localhost;Port=5432;Database=marketnest_dev;Username=postgres;Password=postgres";
            builder.UseNpgsql(conn, b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
            return new AppDbContext(builder.Options);
        }
    }
}
