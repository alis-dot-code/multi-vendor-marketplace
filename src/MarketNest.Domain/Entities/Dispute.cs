using MarketNest.Domain.Common;
using MarketNest.Domain.Enums;

namespace MarketNest.Domain.Entities;

public class Dispute : AuditableEntity
{
    public Guid BookingId { get; set; }
    public Guid OpenedBy { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DisputeStatus Status { get; set; }
    public Guid? AdminId { get; set; }
    public string? AdminNotes { get; set; }
    public string? Resolution { get; set; }
    public int? RefundAmountCents { get; set; }
    public DateTime? ResolvedAt { get; set; }

    // Navigation properties
    public virtual Booking Booking { get; set; } = null!;
    public virtual User Opener { get; set; } = null!;
    public virtual User? Admin { get; set; }
}
