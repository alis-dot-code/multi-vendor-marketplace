using MarketNest.Domain.Entities;

namespace MarketNest.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IVendorRepository Vendors { get; }
    IListingRepository Listings { get; }
    IBookingRepository Bookings { get; }
    IPaymentRepository Payments { get; }
    IReviewRepository Reviews { get; }
    ICategoryRepository Categories { get; }
    IDisputeRepository Disputes { get; }
    INotificationRepository Notifications { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    Task<int> SaveChangesAsync();
}
