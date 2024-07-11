using System.Security.Claims;

namespace MarketNest.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(Domain.Entities.User user);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
