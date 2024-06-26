using MarketNest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketNest.Infrastructure.Persistence.Configurations;

public class DisputeConfiguration : IEntityTypeConfiguration<Dispute>
{
    public void Configure(EntityTypeBuilder<Dispute> builder)
    {
        builder.ToTable("disputes");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).ValueGeneratedOnAdd();
        
        builder.Property(d => d.Reason).IsRequired().HasMaxLength(1000);
        builder.Property(d => d.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(d => d.AdminNotes).HasMaxLength(1000);
        builder.Property(d => d.Resolution).HasMaxLength(2000);
        builder.Property(d => d.RefundAmountCents);
        
        builder.HasOne(d => d.Booking).WithMany(b => b.Disputes).HasForeignKey(d => d.BookingId);
        builder.HasOne(d => d.Opener).WithMany(u => u.OpenedDisputes).HasForeignKey(d => d.OpenedBy);
        builder.HasOne(d => d.Admin).WithMany().HasForeignKey(d => d.AdminId);
    }
}
