using AutoMapper;
using FluentValidation;
using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Interfaces.Repositories;
using MarketNest.Application.Common.Interfaces;

namespace MarketNest.Application.Auth;

public record GoogleLoginCommand(string IdToken) : IRequest<AuthResponseDto>;

public class GoogleLoginValidator : AbstractValidator<GoogleLoginCommand>
{
    public GoogleLoginValidator()
    {
        RuleFor(x => x.IdToken).NotEmpty();
    }
}

public class GoogleLoginHandler : IRequestHandler<GoogleLoginCommand, AuthResponseDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IJwtTokenService _jwt;
    private readonly IMapper _mapper;
    private readonly IGoogleAuthService _google;

    public GoogleLoginHandler(IUnitOfWork uow, IJwtTokenService jwt, IMapper mapper, IGoogleAuthService google)
    {
        _uow = uow;
        _jwt = jwt;
        _mapper = mapper;
        _google = google;
    }

    public async Task<AuthResponseDto> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
    {
        var payload = await _google.ValidateIdTokenAsync(request.IdToken);

        var user = await _uow.Users.FindByGoogleIdAsync(payload.Subject);
        if (user == null)
        {
            user = await _uow.Users.FindByEmailAsync(payload.Email);
        }

        if (user == null)
        {
            user = new User
            {
                Email = payload.Email,
                FirstName = payload.GivenName ?? string.Empty,
                LastName = payload.FamilyName ?? string.Empty,
                GoogleId = payload.Subject,
                EmailVerified = payload.EmailVerified,
                Role = Domain.Enums.UserRole.Buyer,
                IsActive = true
            };
            await _uow.Users.AddAsync(user);
        }
        else
        {
            if (string.IsNullOrEmpty(user.GoogleId)) user.GoogleId = payload.Subject;
        }

        // rotate refresh tokens
        var refreshTokenString = _jwt.GenerateRefreshToken();
        var refreshDays = 7;

        var existing = await _uow.RefreshTokens.GetByUserIdAsync(user.Id);
        foreach (var r in existing) r.RevokedAt = DateTime.UtcNow;

        var refresh = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshTokenString,
            ExpiresAt = DateTime.UtcNow.AddDays(refreshDays)
        };
        await _uow.RefreshTokens.AddAsync(refresh);

        await _uow.SaveChangesAsync();

        var accessToken = _jwt.GenerateAccessToken(user);
        var expires = DateTime.UtcNow.AddMinutes(15);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenString,
            ExpiresAt = expires,
            User = _mapper.Map<UserDto>(user)
        };
    }
}
