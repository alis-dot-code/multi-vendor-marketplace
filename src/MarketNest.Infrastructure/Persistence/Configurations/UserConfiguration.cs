using MarketNest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketNest.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();
        
        builder.Property(u => u.Email).HasMaxLength(256).IsRequired();
        builder.HasIndex(u => u.Email).IsUnique();
        
        builder.Property(u => u.PasswordHash).HasMaxLength(512);
        builder.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(u => u.LastName).HasMaxLength(100).IsRequired();
        builder.Property(u => u.Phone).HasMaxLength(20);
        builder.Property(u => u.AvatarUrl).HasMaxLength(512);
        builder.Property(u => u.GoogleId).HasMaxLength(256);
        
        builder.Property(u => u.Role).HasConversion<string>().HasMaxLength(20);
        
        builder.HasMany(u => u.Bookings).WithOne(b => b.Buyer).HasForeignKey(b => b.BuyerId);
        builder.HasMany(u => u.Reviews).WithOne(r => r.Buyer).HasForeignKey(r => r.BuyerId);
        builder.HasMany(u => u.Notifications).WithOne(n => n.User).HasForeignKey(n => n.UserId);
        builder.HasMany(u => u.OpenedDisputes).WithOne(d => d.Opener).HasForeignKey(d => d.OpenedBy);
        
        builder.HasOne(u => u.Vendor).WithOne(v => v.User).HasForeignKey<Domain.Entities.Vendor>(v => v.UserId);
        builder.HasOne(u => u.RefreshToken).WithOne(r => r.User).HasForeignKey<RefreshToken>(r => r.UserId);
    }
}
