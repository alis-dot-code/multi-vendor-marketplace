using FluentValidation;
using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Enums;
using MarketNest.Domain.Interfaces.Repositories;
using AutoMapper;

namespace MarketNest.Application.Bookings;

public record CancelBookingCommand(Guid BookingId, string? Reason) : IRequest<BookingDto>;

public class CancelBookingValidator : AbstractValidator<CancelBookingCommand>
{
    public CancelBookingValidator()
    {
        RuleFor(x => x.BookingId).NotEmpty();
    }
}

public class CancelBookingHandler : IRequestHandler<CancelBookingCommand, BookingDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _current;
    private readonly IMapper _mapper;
    private readonly IStripeService _stripe;

    public CancelBookingHandler(IUnitOfWork uow, ICurrentUserService current, IMapper mapper, IStripeService stripe)
    {
        _uow = uow;
        _current = current;
        _mapper = mapper;
        _stripe = stripe;
    }

    public async Task<BookingDto> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _uow.Bookings.GetByIdAsync(request.BookingId) ?? throw new NotFoundException("Booking not found");

        var isBuyer = booking.BuyerId == _current.UserId;
        var listing = await _uow.Listings.GetByIdAsync(booking.ListingId) ?? throw new NotFoundException("Listing not found");
        var vendor = await _uow.Vendors.GetByIdAsync(listing.VendorId) ?? throw new NotFoundException("Vendor not found");
        var isVendor = vendor.UserId == _current.UserId;

        if (!isBuyer && !isVendor) throw new ForbiddenException("Only buyer or vendor can cancel booking");
        if (!(booking.Status == BookingStatus.Pending || booking.Status == BookingStatus.Confirmed)) throw new ConflictException("Only pending or confirmed bookings can be cancelled");

        booking.Status = isBuyer ? BookingStatus.CancelledByBuyer : BookingStatus.CancelledByVendor;
        booking.CancelledAt = DateTime.UtcNow;
        booking.CancellationReason = request.Reason;
        _uow.Bookings.Update(booking);

        // release slot
        var listingEntity = await _uow.Listings.GetByIdAsync(booking.ListingId);
        var slot = listingEntity.AvailabilitySlots.FirstOrDefault(s => s.Id == booking.SlotId);
        if (slot != null) slot.IsBooked = false;

        // attempt refund if payment exists and succeeded
        var payment = await _uow.Payments.GetByBookingIdAsync(booking.Id);
        if (payment != null && payment.Status == MarketNest.Domain.Enums.PaymentStatus.Succeeded)
        {
            // call stripe refund for full amount
            var refundId = await _stripe.ProcessRefund(payment.StripePaymentIntentId, payment.AmountCents);
            payment.RefundAmountCents = payment.AmountCents;
            payment.StripeRefundId = refundId;
            payment.Status = MarketNest.Domain.Enums.PaymentStatus.Refunded;
            _uow.Payments.Update(payment);
        }

        await _uow.SaveChangesAsync();

        // notify other party
        var notifyUser = isBuyer ? vendor.UserId : booking.BuyerId;
        await _uow.Notifications.AddAsync(new Notification
        {
            UserId = notifyUser,
            Type = MarketNest.Domain.Enums.NotificationType.BookingCancelled,
            Title = "Booking cancelled",
            Message = $"Booking {booking.BookingNumber} was cancelled"
        });
        await _uow.SaveChangesAsync();

        return _mapper.Map<BookingDto>(booking);
    }
}
