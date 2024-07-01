using MarketNest.Domain.Entities;
using MarketNest.Domain.Interfaces.Repositories;

namespace MarketNest.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IUserRepository? _users;
    private IVendorRepository? _vendors;
    private IListingRepository? _listings;
    private IBookingRepository? _bookings;
    private IPaymentRepository? _payments;
    private IReviewRepository? _reviews;
    private ICategoryRepository? _categories;
    private IDisputeRepository? _disputes;
    private INotificationRepository? _notifications;
    private IRefreshTokenRepository? _refreshTokens;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IUserRepository Users => _users ??= new UserRepository(_context);
    public IVendorRepository Vendors => _vendors ??= new VendorRepository(_context);
    public IListingRepository Listings => _listings ??= new ListingRepository(_context);
    public IBookingRepository Bookings => _bookings ??= new BookingRepository(_context);
    public IPaymentRepository Payments => _payments ??= new PaymentRepository(_context);
    public IReviewRepository Reviews => _reviews ??= new ReviewRepository(_context);
    public ICategoryRepository Categories => _categories ??= new CategoryRepository(_context);
    public IDisputeRepository Disputes => _disputes ??= new DisputeRepository(_context);
    public INotificationRepository Notifications => _notifications ??= new NotificationRepository(_context);
    public IRefreshTokenRepository RefreshTokens => _refreshTokens ??= new RefreshTokenRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
