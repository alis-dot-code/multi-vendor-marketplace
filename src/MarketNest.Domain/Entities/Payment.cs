using MarketNest.Domain.Common;
using MarketNest.Domain.Enums;

namespace MarketNest.Domain.Entities;

public class Payment : AuditableEntity
{
    public Guid BookingId { get; set; }
    public string StripePaymentIntentId { get; set; } = string.Empty;
    public string? StripeChargeId { get; set; }
    public string? StripeTransferId { get; set; }
    public int AmountCents { get; set; }
    public int PlatformFeeCents { get; set; }
    public string Currency { get; set; } = "USD";
    public PaymentStatus Status { get; set; }
    public int? RefundAmountCents { get; set; }
    public string? RefundReason { get; set; }
    public string? StripeRefundId { get; set; }
    public string? Metadata { get; set; }

    // Navigation properties
    public virtual Booking Booking { get; set; } = null!;
}
