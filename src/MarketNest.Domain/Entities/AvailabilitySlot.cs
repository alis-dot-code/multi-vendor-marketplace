using MarketNest.Domain.Common;

namespace MarketNest.Domain.Entities;

public class AvailabilitySlot : AuditableEntity
{
    public Guid ListingId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsBooked { get; set; }
    public bool IsBlocked { get; set; }
    public string? RecurrenceRule { get; set; }

    // Navigation properties
    public virtual Listing Listing { get; set; } = null!;
    public virtual Booking? Booking { get; set; }
}
