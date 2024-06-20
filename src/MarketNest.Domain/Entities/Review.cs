using MarketNest.Domain.Common;

namespace MarketNest.Domain.Entities;

public class Review : AuditableEntity
{
    public Guid BookingId { get; set; }
    public Guid BuyerId { get; set; }
    public Guid ListingId { get; set; }
    public Guid VendorId { get; set; }
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string Comment { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    public bool IsFlagged { get; set; }
    public bool AdminHidden { get; set; }

    // Navigation properties
    public virtual Booking Booking { get; set; } = null!;
    public virtual User Buyer { get; set; } = null!;
    public virtual Listing Listing { get; set; } = null!;
    public virtual Vendor Vendor { get; set; } = null!;
    public virtual ReviewReply? Reply { get; set; }
}
