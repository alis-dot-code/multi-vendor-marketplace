using MarketNest.Domain.Common;

namespace MarketNest.Domain.Entities;

public class ListingImage : AuditableEntity
{
    public Guid ListingId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string PublicId { get; set; } = string.Empty;
    public string? AltText { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }

    // Navigation properties
    public virtual Listing Listing { get; set; } = null!;
}
