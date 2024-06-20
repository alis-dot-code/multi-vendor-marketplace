using MarketNest.Domain.Entities;

namespace MarketNest.Domain.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> FindByEmailAsync(string email);
    Task<User?> FindByGoogleIdAsync(string googleId);
}
