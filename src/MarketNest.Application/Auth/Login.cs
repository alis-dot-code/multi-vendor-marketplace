using AutoMapper;
using FluentValidation;
using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Domain.Interfaces.Repositories;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Entities;

namespace MarketNest.Application.Auth;

public record LoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class LoginHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IJwtTokenService _jwt;
    private readonly IMapper _mapper;

    public LoginHandler(IUnitOfWork uow, IJwtTokenService jwt, IMapper mapper)
    {
        _uow = uow;
        _jwt = jwt;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _uow.Users.FindByEmailAsync(request.Email);
        if (user == null) throw new BadRequestException("Invalid credentials");

        if (string.IsNullOrEmpty(user.PasswordHash) || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new BadRequestException("Invalid credentials");

        // rotate refresh token: create new and revoke old ones
        var refreshTokenString = _jwt.GenerateRefreshToken();
        var refreshDays = 7;

        var existing = await _uow.RefreshTokens.GetByUserIdAsync(user.Id);
        foreach (var r in existing)
        {
            r.RevokedAt = DateTime.UtcNow;
        }

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
