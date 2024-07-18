using AutoMapper;
using FluentValidation;
using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Interfaces.Repositories;
using MarketNest.Application.Common.Interfaces;

namespace MarketNest.Application.Auth;

public record RegisterCommand(string Email, string Password, string FirstName, string LastName) : IRequest<AuthResponseDto>;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}

public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IJwtTokenService _jwt;
    private readonly IMapper _mapper;

    public RegisterHandler(IUnitOfWork uow, IJwtTokenService jwt, IMapper mapper)
    {
        _uow = uow;
        _jwt = jwt;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existing = await _uow.Users.FindByEmailAsync(request.Email);
        if (existing != null) throw new ConflictException("Email already in use");

        var user = new User
        {
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = Domain.Enums.UserRole.Buyer,
            EmailVerified = false,
            IsActive = true
        };

        await _uow.Users.AddAsync(user);

        // create refresh token
        var refreshTokenString = _jwt.GenerateRefreshToken();
        var refreshDays = 7;
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
