using MarketNest.Domain.Common;

namespace MarketNest.Domain.Entities;

public class PlatformSetting : AuditableEntity
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Description { get; set; }
}
