using MarketNest.Domain.Common;
using MarketNest.Domain.Enums;

namespace MarketNest.Domain.Entities;

public class Vendor : AuditableEntity
{
    public Guid UserId { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string? BannerUrl { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? ZipCode { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public VendorStatus Status { get; set; }
    public string? StripeAccountId { get; set; }
    public bool StripeOnboardingDone { get; set; }
    public ConfirmMode ConfirmMode { get; set; } = ConfirmMode.Auto;
    public decimal AvgRating { get; set; }
    public int TotalReviews { get; set; }
    public string? AdminNotes { get; set; }
    public DateTime? ApprovedAt { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<ReviewReply> ReviewReplies { get; set; } = new List<ReviewReply>();
}
