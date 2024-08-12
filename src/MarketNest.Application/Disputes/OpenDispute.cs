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
    public static class OpenDispute
    {
        public record Command(Guid BookingId, string Reason) : IRequest<Result<Dispute>>;

        public class Handler : IRequestHandler<Command, Result<Dispute>>
        {
            private readonly IUnitOfWork _uow;
            private readonly ICurrentUserService _currentUser;
            private readonly IEmailService _emailService;

            public Handler(IUnitOfWork uow, ICurrentUserService currentUser, IEmailService emailService)
            {
                _uow = uow;
                _currentUser = currentUser;
                _emailService = emailService;
            }

            public async Task<Result<Dispute>> Handle(Command request, CancellationToken cancellationToken)
            {
                var booking = await _uow.Bookings.GetByIdAsync(request.BookingId);
                if (booking == null) return Result<Dispute>.Failure("Booking not found");
                if (booking.BuyerId != _currentUser.UserId && booking.VendorId != _currentUser.UserId) return Result<Dispute>.Failure("Forbidden");
                if (booking.Status != BookingStatus.Confirmed && booking.Status != BookingStatus.Completed) return Result<Dispute>.Failure("Booking not eligible for dispute");

                booking.Status = BookingStatus.Disputed;
                _uow.Bookings.Update(booking);

                var dispute = new Dispute
                {
                    BookingId = booking.Id,
                    OpenedBy = _currentUser.UserId,
                    Reason = request.Reason,
                    Status = DisputeStatus.Open
                };

                await _uow.Disputes.AddAsync(dispute);
                await _uow.SaveChangesAsync();

                // notify other party
                var otherUserId = booking.BuyerId == _currentUser.UserId ? booking.VendorId : booking.BuyerId;
                var other = await _uow.Users.GetByIdAsync(otherUserId);
                if (other != null)
                {
                    await _emailService.SendEmailAsync(other.Email, "A dispute was opened", $"A dispute was opened for booking {booking.BookingNumber}: {dispute.Reason}");
                    await _uow.Notifications.AddAsync(new Notification
                    {
                        UserId = other.Id,
                        Type = NotificationType.DisputeOpened,
                        Title = "A dispute was opened",
                        Message = $"A dispute was opened for booking {booking.BookingNumber}",
                        IsRead = false
                    });
                    await _uow.SaveChangesAsync();
                }

                return Result<Dispute>.Success(dispute);
            }
        }
    }
}
