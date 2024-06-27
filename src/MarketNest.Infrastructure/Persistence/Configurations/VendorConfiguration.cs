using MarketNest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketNest.Infrastructure.Persistence.Configurations;

public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.ToTable("vendors");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).ValueGeneratedOnAdd();
        
        builder.HasIndex(v => v.UserId).IsUnique();
        builder.HasIndex(v => v.Slug).IsUnique();
        
        builder.Property(v => v.BusinessName).HasMaxLength(200).IsRequired();
        builder.Property(v => v.Slug).HasMaxLength(200).IsRequired();
        builder.Property(v => v.Description).IsRequired();
        builder.Property(v => v.LogoUrl).HasMaxLength(512);
        builder.Property(v => v.BannerUrl).HasMaxLength(512);
        builder.Property(v => v.Address).HasMaxLength(500);
        builder.Property(v => v.City).HasMaxLength(100);
        builder.Property(v => v.State).HasMaxLength(100);
        builder.Property(v => v.Country).HasMaxLength(100);
        builder.Property(v => v.ZipCode).HasMaxLength(20);
        builder.Property(v => v.Latitude).HasPrecision(10, 7);
        builder.Property(v => v.Longitude).HasPrecision(10, 7);
        builder.Property(v => v.Phone).HasMaxLength(20);
        builder.Property(v => v.Website).HasMaxLength(512);
        
        builder.Property(v => v.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(v => v.StripeAccountId).HasMaxLength(256);
        builder.Property(v => v.ConfirmMode).HasConversion<string>().HasMaxLength(20);
        builder.Property(v => v.AvgRating).HasPrecision(3, 2);
        builder.Property(v => v.AdminNotes).HasMaxLength(1000);
        
        builder.HasOne(v => v.User).WithOne(u => u.Vendor).HasForeignKey<Vendor>(v => v.UserId);
        builder.HasMany(v => v.Listings).WithOne(l => l.Vendor).HasForeignKey(l => l.VendorId);
        builder.HasMany(v => v.Bookings).WithOne(b => b.Vendor).HasForeignKey(b => b.VendorId);
        builder.HasMany(v => v.Reviews).WithOne(r => r.Vendor).HasForeignKey(r => r.VendorId);
        builder.HasMany(v => v.ReviewReplies).WithOne(r => r.Vendor).HasForeignKey(r => r.VendorId);
    }
}
