using AutoMapper;
using FluentValidation;
using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Domain.Interfaces.Repositories;
using MarketNest.Application.Common.Interfaces;

namespace MarketNest.Application.Auth;

public record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<AuthResponseDto>;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.AccessToken).NotEmpty();
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IJwtTokenService _jwt;
    private readonly IMapper _mapper;

    public RefreshTokenHandler(IUnitOfWork uow, IJwtTokenService jwt, IMapper mapper)
    {
        _uow = uow;
        _jwt = jwt;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = _jwt.GetPrincipalFromExpiredToken(request.AccessToken);
        var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId)) throw new BadRequestException("Invalid token principal");

        var stored = await _uow.RefreshTokens.GetByTokenAsync(request.RefreshToken);
        if (stored == null || stored.UserId != userId) throw new BadRequestException("Invalid refresh token");
        if (stored.RevokedAt != null) {
            // revoke all user tokens
            var all = await _uow.RefreshTokens.GetByUserIdAsync(userId);
            foreach (var t in all) t.RevokedAt = DateTime.UtcNow;
            await _uow.SaveChangesAsync();
            throw new BadRequestException("Refresh token reuse detected");
        }

        if (stored.ExpiresAt < DateTime.UtcNow) throw new BadRequestException("Refresh token expired");

        var user = await _uow.Users.GetByIdAsync(userId);
        if (user == null) throw new NotFoundException("User not found");

        // rotate
        stored.RevokedAt = DateTime.UtcNow;
        var newRefresh = _jwt.GenerateRefreshToken();
        var refreshDays = 7;
        var newEntry = new MarketNest.Domain.Entities.RefreshToken
        {
            UserId = user.Id,
            Token = newRefresh,
            ExpiresAt = DateTime.UtcNow.AddDays(refreshDays),
            ReplacedBy = null
        };
        stored.ReplacedBy = newEntry.Id;
        await _uow.RefreshTokens.AddAsync(newEntry);

        await _uow.SaveChangesAsync();

        var accessToken = _jwt.GenerateAccessToken(user);
        var expires = DateTime.UtcNow.AddMinutes(15);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefresh,
            ExpiresAt = expires,
            User = _mapper.Map<MarketNest.Application.Common.DTOs.UserDto>(user)
        };
    }
}
