using FluentValidation;
using MediatR;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Domain.Interfaces.Repositories;

namespace MarketNest.Application.Auth;

public record RevokeTokenCommand(string RefreshToken) : IRequest<Unit>;

public class RevokeTokenValidator : AbstractValidator<RevokeTokenCommand>
{
    public RevokeTokenValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}

public class RevokeTokenHandler : IRequestHandler<RevokeTokenCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public RevokeTokenHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Unit> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var stored = await _uow.RefreshTokens.GetByTokenAsync(request.RefreshToken);
        if (stored == null) throw new NotFoundException("Refresh token not found");
        stored.RevokedAt = DateTime.UtcNow;
        await _uow.SaveChangesAsync();
        return Unit.Value;
    }
}
