using MarketNest.Domain.Common;
using MarketNest.Domain.Enums;

namespace MarketNest.Domain.Entities;

public class Booking : AuditableEntity
{
    public string BookingNumber { get; set; } = string.Empty;
    public Guid BuyerId { get; set; }
    public Guid ListingId { get; set; }
    public Guid VendorId { get; set; }
    public Guid SlotId { get; set; }
    public BookingStatus Status { get; set; }
    public int Attendees { get; set; }
    public int TotalCents { get; set; }
    public int PlatformFeeCents { get; set; }
    public int VendorAmountCents { get; set; }
    public string? BuyerNotes { get; set; }
    public string? VendorNotes { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? CancellationReason { get; set; }

    // Navigation properties
    public virtual User Buyer { get; set; } = null!;
    public virtual Listing Listing { get; set; } = null!;
    public virtual Vendor Vendor { get; set; } = null!;
    public virtual AvailabilitySlot Slot { get; set; } = null!;
    public virtual Payment? Payment { get; set; }
    public virtual Review? Review { get; set; }
    public virtual ICollection<Dispute> Disputes { get; set; } = new List<Dispute>();
}
