using System;
using System.Threading;
using System.Threading.Tasks;
using MarketNest.Application.Common.Models;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Enums;
using MarketNest.Domain.Interfaces.Repositories;
using MarketNest.Application.Common.Interfaces;
using MediatR;

namespace MarketNest.Application.Disputes
{
    public static class ResolveDispute
    {
        public record Command(Guid DisputeId, DisputeStatus Status, string Resolution, int? RefundAmountCents) : IRequest<Result<bool>>;

        public class Handler : IRequestHandler<Command, Result<bool>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IEmailService _emailService;
            private readonly IStripeService _stripeService;

            public Handler(IUnitOfWork uow, IEmailService emailService, IStripeService stripeService)
            {
                _uow = uow;
                _emailService = emailService;
                _stripeService = stripeService;
            }

            public async Task<Result<bool>> Handle(Command request, CancellationToken cancellationToken)
            {
                var dispute = await _uow.Disputes.GetByIdAsync(request.DisputeId);
                if (dispute == null) return Result<bool>.Failure("Dispute not found");

                dispute.Status = request.Status;
                dispute.Resolution = request.Resolution;
                dispute.ResolvedAt = DateTime.UtcNow;
                _uow.Disputes.Update(dispute);

                if (request.RefundAmountCents.HasValue && request.RefundAmountCents.Value > 0)
                {
                    var payment = await _uow.Payments.GetByBookingIdAsync(dispute.BookingId);
                    if (payment != null && payment.Status == PaymentStatus.Succeeded)
                    {
                        await _stripeService.ProcessRefund(payment.StripePaymentIntentId, request.RefundAmountCents.Value);
                        payment.RefundAmountCents = request.RefundAmountCents.Value;
                        payment.Status = PaymentStatus.PartiallyRefunded;
                        _uow.Payments.Update(payment);
                    }
                }

                await _uow.SaveChangesAsync();

                var booking = await _uow.Bookings.GetByIdAsync(dispute.BookingId);
                var otherUserId = booking.BuyerId == dispute.OpenedBy ? booking.VendorId : booking.BuyerId;
                var other = await _uow.Users.GetByIdAsync(otherUserId);
                if (other != null)
                {
                    await _emailService.SendEmailAsync(other.Email, "Dispute resolved", $"Dispute resolved: {dispute.Resolution}");
                    await _uow.Notifications.AddAsync(new Notification
                    {
                        UserId = other.Id,
                        Type = NotificationType.DisputeResolved,
                        Title = "Dispute resolved",
                        Message = $"Dispute for booking {booking.BookingNumber} has been resolved",
                        IsRead = false
                    });
                    await _uow.SaveChangesAsync();
                }

                return Result<bool>.Success(true);
            }
        }
    }
}
