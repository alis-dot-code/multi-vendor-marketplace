using MarketNest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketNest.Infrastructure.Persistence.Configurations;

public class ListingConfiguration : IEntityTypeConfiguration<Listing>
{
    public void Configure(EntityTypeBuilder<Listing> builder)
    {
        builder.ToTable("listings");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).ValueGeneratedOnAdd();
        
        builder.HasIndex(l => new { l.VendorId, l.Slug }).IsUnique();
        
        builder.Property(l => l.Title).HasMaxLength(200).IsRequired();
        builder.Property(l => l.Slug).HasMaxLength(200).IsRequired();
        builder.Property(l => l.Description).IsRequired();
        builder.Property(l => l.ShortDescription).HasMaxLength(500);
        builder.Property(l => l.PriceCents).IsRequired();
        builder.Property(l => l.Currency).HasMaxLength(3).HasDefaultValue("USD");
        builder.Property(l => l.DurationMinutes).IsRequired();
        builder.Property(l => l.MaxAttendees).HasDefaultValue(1);
        builder.Property(l => l.LocationText).HasMaxLength(500);
        builder.Property(l => l.VirtualLink).HasMaxLength(512);
        
        builder.Property(l => l.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(l => l.AvgRating).HasPrecision(3, 2);
        
        builder.HasOne(l => l.Vendor).WithMany(v => v.Listings).HasForeignKey(l => l.VendorId);
        builder.HasOne(l => l.Category).WithMany(c => c.Listings).HasForeignKey(l => l.CategoryId);
        builder.HasMany(l => l.Images).WithOne(i => i.Listing).HasForeignKey(i => i.ListingId);
        builder.HasMany(l => l.AvailabilitySlots).WithOne(s => s.Listing).HasForeignKey(s => s.ListingId);
        builder.HasMany(l => l.Bookings).WithOne(b => b.Listing).HasForeignKey(b => b.ListingId);
        builder.HasMany(l => l.Reviews).WithOne(r => r.Listing).HasForeignKey(r => r.ListingId);
    }
}
