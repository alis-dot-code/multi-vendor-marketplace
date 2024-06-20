using MarketNest.Domain.Entities;

namespace MarketNest.Domain.Interfaces.Repositories;

public interface IPaymentRepository : IGenericRepository<Payment>
{
    Task<Payment?> GetByBookingIdAsync(Guid bookingId);
    Task<Payment?> GetByStripePaymentIntentIdAsync(string stripePaymentIntentId);
}
