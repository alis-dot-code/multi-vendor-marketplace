using MarketNest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketNest.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();
        
        builder.Property(p => p.StripePaymentIntentId).HasMaxLength(256).IsRequired();
        builder.HasIndex(p => p.StripePaymentIntentId).IsUnique();
        
        builder.Property(p => p.StripeChargeId).HasMaxLength(256);
        builder.Property(p => p.StripeTransferId).HasMaxLength(256);
        builder.Property(p => p.AmountCents);
        builder.Property(p => p.PlatformFeeCents);
        builder.Property(p => p.Currency).HasMaxLength(3).HasDefaultValue("USD");
        builder.Property(p => p.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(p => p.RefundAmountCents);
        builder.Property(p => p.RefundReason).HasMaxLength(500);
        builder.Property(p => p.StripeRefundId).HasMaxLength(256);
        builder.Property(p => p.Metadata).HasColumnType("jsonb");
        
        builder.HasOne(p => p.Booking).WithOne(b => b.Payment).HasForeignKey<Payment>(p => p.BookingId);
    }
}
