using MarketNest.Domain.Common;
using MarketNest.Domain.Enums;

namespace MarketNest.Domain.Entities;

public class User : AuditableEntity
{
    public string Email { get; set; } = string.Empty;
    public string? PasswordHash { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? AvatarUrl { get; set; }
    public UserRole Role { get; set; }
    public string? GoogleId { get; set; }
    public bool EmailVerified { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual RefreshToken? RefreshToken { get; set; }
    public virtual Vendor? Vendor { get; set; }
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public virtual ICollection<Dispute> OpenedDisputes { get; set; } = new List<Dispute>();
}
