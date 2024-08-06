using FluentValidation;
using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Enums;
using MarketNest.Domain.Interfaces.Repositories;

namespace MarketNest.Application.Payments;

public record CreatePaymentIntentCommand(Guid BookingId) : IRequest<PaymentIntentDto>;

public class CreatePaymentIntentValidator : AbstractValidator<CreatePaymentIntentCommand>
{
    public CreatePaymentIntentValidator()
    {
        RuleFor(x => x.BookingId).NotEmpty();
    }
}

public class CreatePaymentIntentHandler : IRequestHandler<CreatePaymentIntentCommand, PaymentIntentDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IStripeService _stripe;

    public CreatePaymentIntentHandler(IUnitOfWork uow, IStripeService stripe)
    {
        _uow = uow;
        _stripe = stripe;
    }

    public async Task<PaymentIntentDto> Handle(CreatePaymentIntentCommand request, CancellationToken cancellationToken)
    {
        var booking = await _uow.Bookings.GetByIdAsync(request.BookingId) ?? throw new NotFoundException("Booking not found");
        if (booking.TotalCents <= 0) throw new BadRequestException("Invalid amount");

        var listing = await _uow.Listings.GetByIdAsync(booking.ListingId) ?? throw new NotFoundException("Listing not found");
        var vendor = await _uow.Vendors.GetByIdAsync(listing.VendorId) ?? throw new NotFoundException("Vendor not found");

        var currency = "USD";
        var metadata = new Dictionary<string, string>
        {
            { "bookingId", booking.Id.ToString() },
            { "buyerId", booking.BuyerId.ToString() }
        };

        var (clientSecret, intentId) = await _stripe.CreatePaymentIntent(booking.TotalCents, currency, vendor.StripeAccountId ?? string.Empty, booking.PlatformFeeCents, metadata);

        var payment = new Payment
        {
            BookingId = booking.Id,
            StripePaymentIntentId = intentId,
            AmountCents = booking.TotalCents,
            PlatformFeeCents = booking.PlatformFeeCents,
            Currency = currency,
            Status = PaymentStatus.Pending,
            Metadata = System.Text.Json.JsonSerializer.Serialize(metadata)
        };

        await _uow.Payments.AddAsync(payment);
        await _uow.SaveChangesAsync();

        return new PaymentIntentDto { ClientSecret = clientSecret, PaymentIntentId = intentId };
    }
}
