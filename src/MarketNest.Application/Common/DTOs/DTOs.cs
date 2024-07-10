namespace MarketNest.Application.Common.DTOs;

public class AuthResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserDto User { get; set; } = new();
}

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? AvatarUrl { get; set; }
    public string Role { get; set; } = string.Empty;
}

public class VendorDto
{
    public Guid Id { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public decimal AvgRating { get; set; }
    public int TotalReviews { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class VendorDashboardDto
{
    public int TotalEarningsCents { get; set; }
    public int ThisMonthEarningsCents { get; set; }
    public int TotalBookings { get; set; }
    public int PendingBookings { get; set; }
    public decimal AvgRating { get; set; }
    public List<BookingDto> RecentBookings { get; set; } = new();
}

public class ListingDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int PriceCents { get; set; }
    public int DurationMinutes { get; set; }
    public string? PrimaryImageUrl { get; set; }
    public decimal AvgRating { get; set; }
    public int TotalReviews { get; set; }
    public string VendorName { get; set; } = string.Empty;
    public string VendorSlug { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
}

public class ListingDetailDto : ListingDto
{
    public string Description { get; set; } = string.Empty;
    public List<ListingImageDto> Images { get; set; } = new();
    public bool IsVirtual { get; set; }
    public string? VirtualLink { get; set; }
    public VendorDto? Vendor { get; set; }
}

public class ListingImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}

public class ListingSearchResultDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int PriceCents { get; set; }
    public string? PrimaryImageUrl { get; set; }
    public decimal AvgRating { get; set; }
    public int TotalReviews { get; set; }
    public string VendorName { get; set; } = string.Empty;
    public string VendorSlug { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
}

public class BookingDto
{
    public Guid Id { get; set; }
    public string BookingNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string ListingTitle { get; set; } = string.Empty;
    public DateTime SlotStart { get; set; }
    public DateTime SlotEnd { get; set; }
    public int TotalCents { get; set; }
    public int PlatformFeeCents { get; set; }
    public int Attendees { get; set; }
}

public class AvailabilitySlotDto
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsBooked { get; set; }
    public bool IsBlocked { get; set; }
}

public class PaymentDto
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public int AmountCents { get; set; }
    public int PlatformFeeCents { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class PaymentIntentDto
{
    public string ClientSecret { get; set; } = string.Empty;
    public string PaymentIntentId { get; set; } = string.Empty;
}

public class ReviewDto
{
    public Guid Id { get; set; }
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string BuyerName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public ReviewReplyDto? Reply { get; set; }
}

public class ReviewReplyDto
{
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public List<CategoryDto> Children { get; set; } = new();
}

public class DisputeDto
{
    public Guid Id { get; set; }
    public string BookingNumber { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Resolution { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class NotificationDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PlatformSettingDto
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class AnalyticsDto
{
    public int TotalGmvCents { get; set; }
    public int TotalRevenueCents { get; set; }
    public int TotalBookings { get; set; }
    public int ActiveVendors { get; set; }
    public int TotalUsers { get; set; }
    public List<MonthlyRevenueDto> RevenueByMonth { get; set; } = new();
}

public class MonthlyRevenueDto
{
    public string Month { get; set; } = string.Empty;
    public int RevenueCents { get; set; }
}
