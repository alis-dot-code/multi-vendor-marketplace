using MarketNest.Domain.Common;

namespace MarketNest.Domain.Entities;

public class ReviewReply : AuditableEntity
{
    public Guid ReviewId { get; set; }
    public Guid VendorId { get; set; }
    public string Comment { get; set; } = string.Empty;

    // Navigation properties
    public virtual Review Review { get; set; } = null!;
    public virtual Vendor Vendor { get; set; } = null!;
}
