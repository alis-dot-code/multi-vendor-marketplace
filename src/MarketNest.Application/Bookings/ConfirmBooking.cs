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

public record ConfirmBookingCommand(Guid BookingId) : IRequest<BookingDto>;

public class ConfirmBookingValidator : AbstractValidator<ConfirmBookingCommand>
{
    public ConfirmBookingValidator()
    {
        RuleFor(x => x.BookingId).NotEmpty();
    }
}

public class ConfirmBookingHandler : IRequestHandler<ConfirmBookingCommand, BookingDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _current;
    private readonly IMapper _mapper;

    public ConfirmBookingHandler(IUnitOfWork uow, ICurrentUserService current, IMapper mapper)
    {
        _uow = uow;
        _current = current;
        _mapper = mapper;
    }

    public async Task<BookingDto> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _uow.Bookings.GetByIdAsync(request.BookingId) ?? throw new NotFoundException("Booking not found");
        var listing = await _uow.Listings.GetByIdAsync(booking.ListingId) ?? throw new NotFoundException("Listing not found");
        var vendor = await _uow.Vendors.GetByIdAsync(listing.VendorId) ?? throw new NotFoundException("Vendor not found");

        if (vendor.UserId != _current.UserId) throw new ForbiddenException("Only vendor can confirm booking");
        if (booking.Status != BookingStatus.Pending) throw new ConflictException("Only pending bookings can be confirmed");

        booking.Status = BookingStatus.Confirmed;
        booking.ConfirmedAt = DateTime.UtcNow;
        _uow.Bookings.Update(booking);
        await _uow.SaveChangesAsync();

        await _uow.Notifications.AddAsync(new Notification
        {
            UserId = booking.BuyerId,
            Type = MarketNest.Domain.Enums.NotificationType.BookingConfirmed,
            Title = "Booking confirmed",
            Message = $"Your booking {booking.BookingNumber} has been confirmed"
        });
        await _uow.SaveChangesAsync();

        return _mapper.Map<BookingDto>(booking);
    }
}
