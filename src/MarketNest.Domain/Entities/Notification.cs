using MarketNest.Domain.Common;
using MarketNest.Domain.Enums;

namespace MarketNest.Domain.Entities;

public class Notification : AuditableEntity
{
    public Guid UserId { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Data { get; set; }
    public bool IsRead { get; set; }
    public bool EmailSent { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
}
