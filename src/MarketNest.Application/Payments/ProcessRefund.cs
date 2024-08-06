using FluentValidation;
using MediatR;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Interfaces.Repositories;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Enums;

namespace MarketNest.Application.Payments;

public record ProcessRefundCommand(Guid PaymentId) : IRequest<Unit>;

public class ProcessRefundValidator : AbstractValidator<ProcessRefundCommand>
{
    public ProcessRefundValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
    }
}

public class ProcessRefundHandler : IRequestHandler<ProcessRefundCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IStripeService _stripe;

    public ProcessRefundHandler(IUnitOfWork uow, IStripeService stripe)
    {
        _uow = uow;
        _stripe = stripe;
    }

    public async Task<Unit> Handle(ProcessRefundCommand request, CancellationToken cancellationToken)
    {
        var payment = await _uow.Payments.GetByIdAsync(request.PaymentId) ?? throw new NotFoundException("Payment not found");
        if (payment.Status != PaymentStatus.Succeeded) throw new ConflictException("Only succeeded payments can be refunded");

        var refundId = await _stripe.ProcessRefund(payment.StripePaymentIntentId, null);
        payment.StripeRefundId = refundId;
        payment.RefundAmountCents = payment.AmountCents;
        payment.Status = PaymentStatus.Refunded;
        _uow.Payments.Update(payment);
        await _uow.SaveChangesAsync();
        return Unit.Value;
    }
}
