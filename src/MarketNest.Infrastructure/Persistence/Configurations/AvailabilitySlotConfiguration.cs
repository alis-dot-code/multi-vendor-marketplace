using MarketNest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketNest.Infrastructure.Persistence.Configurations;

public class AvailabilitySlotConfiguration : IEntityTypeConfiguration<AvailabilitySlot>
{
    public void Configure(EntityTypeBuilder<AvailabilitySlot> builder)
    {
        builder.ToTable("availability_slots");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();
        
        builder.Property(s => s.StartTime).IsRequired();
        builder.Property(s => s.EndTime).IsRequired();
        builder.Property(s => s.IsBooked);
        builder.Property(s => s.IsBlocked);
        builder.Property(s => s.RecurrenceRule).HasMaxLength(500);
        
        builder.HasCheckConstraint("CK_AvailabilitySlots_EndTime_GreaterThan_StartTime", "end_time > start_time");
        
        builder.HasIndex(s => new { s.ListingId, s.IsBooked, s.IsBlocked })
            .HasFilter("is_booked = false AND is_blocked = false");
        
        builder.HasOne(s => s.Listing).WithMany(l => l.AvailabilitySlots).HasForeignKey(s => s.ListingId);
        builder.HasOne(s => s.Booking).WithOne(b => b.Slot).HasForeignKey<Booking>(b => b.SlotId);
    }
}
