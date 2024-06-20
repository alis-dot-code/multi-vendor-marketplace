using MarketNest.Domain.Common;

namespace MarketNest.Domain.Entities;

public class RefreshToken : AuditableEntity
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public Guid? ReplacedBy { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
}
