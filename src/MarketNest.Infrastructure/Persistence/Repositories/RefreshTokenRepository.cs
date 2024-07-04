using MarketNest.Domain.Entities;
using MarketNest.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MarketNest.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.Set<RefreshToken>().FirstOrDefaultAsync(r => r.Token == token);
    }

    public async Task<IEnumerable<RefreshToken>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Set<RefreshToken>().Where(r => r.UserId == userId).ToListAsync();
    }
}
