namespace MarketNest.Domain.Enums;

public enum BookingStatus
{
    Pending,
    Confirmed,
    CancelledByBuyer,
    CancelledByVendor,
    Completed,
    NoShow,
    Disputed
}
