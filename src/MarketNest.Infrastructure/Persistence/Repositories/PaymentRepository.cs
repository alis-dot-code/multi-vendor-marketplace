using MarketNest.Domain.Entities;
using MarketNest.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MarketNest.Infrastructure.Persistence.Repositories;

public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Payment?> GetByBookingIdAsync(Guid bookingId)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.BookingId == bookingId);
    }

    public async Task<Payment?> GetByStripePaymentIntentIdAsync(string stripePaymentIntentId)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.StripePaymentIntentId == stripePaymentIntentId);
    }
}
