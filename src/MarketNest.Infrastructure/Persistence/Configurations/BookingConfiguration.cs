using MarketNest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketNest.Infrastructure.Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("bookings");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).ValueGeneratedOnAdd();
        
        builder.Property(b => b.BookingNumber).HasMaxLength(50).IsRequired();
        builder.HasIndex(b => b.BookingNumber).IsUnique();
        
        builder.Property(b => b.Status).HasConversion<string>().HasMaxLength(30);
        builder.Property(b => b.Attendees);
        builder.Property(b => b.TotalCents);
        builder.Property(b => b.PlatformFeeCents);
        builder.Property(b => b.VendorAmountCents);
        builder.Property(b => b.BuyerNotes).HasMaxLength(1000);
        builder.Property(b => b.VendorNotes).HasMaxLength(1000);
        builder.Property(b => b.CancellationReason).HasMaxLength(1000);
        
        builder.HasOne(b => b.Buyer).WithMany(u => u.Bookings).HasForeignKey(b => b.BuyerId);
        builder.HasOne(b => b.Listing).WithMany(l => l.Bookings).HasForeignKey(b => b.ListingId);
        builder.HasOne(b => b.Vendor).WithMany(v => v.Bookings).HasForeignKey(b => b.VendorId);
        builder.HasOne(b => b.Slot).WithOne(s => s.Booking).HasForeignKey<Booking>(b => b.SlotId);
        builder.HasOne(b => b.Payment).WithOne(p => p.Booking).HasForeignKey<Payment>(p => p.BookingId);
        builder.HasOne(b => b.Review).WithOne(r => r.Booking).HasForeignKey<Review>(r => r.BookingId);
        builder.HasMany(b => b.Disputes).WithOne(d => d.Booking).HasForeignKey(d => d.BookingId);
    }
}
