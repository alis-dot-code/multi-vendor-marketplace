using MarketNest.Domain.Common;
using MarketNest.Domain.Enums;

namespace MarketNest.Domain.Entities;

public class Listing : AuditableEntity
{
    public Guid VendorId { get; set; }
    public Guid CategoryId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public int PriceCents { get; set; }
    public string Currency { get; set; } = "USD";
    public int DurationMinutes { get; set; }
    public int MaxAttendees { get; set; } = 1;
    public string? LocationText { get; set; }
    public bool IsVirtual { get; set; }
    public string? VirtualLink { get; set; }
    public ListingStatus Status { get; set; }
    public decimal AvgRating { get; set; }
    public int TotalReviews { get; set; }
    public int TotalBookings { get; set; }

    // Navigation properties
    public virtual Vendor Vendor { get; set; } = null!;
    public virtual Category Category { get; set; } = null!;
    public virtual ICollection<ListingImage> Images { get; set; } = new List<ListingImage>();
    public virtual ICollection<AvailabilitySlot> AvailabilitySlots { get; set; } = new List<AvailabilitySlot>();
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
