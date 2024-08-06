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

public record CompleteBookingCommand(Guid BookingId) : IRequest<BookingDto>;

public class CompleteBookingValidator : AbstractValidator<CompleteBookingCommand>
{
    public CompleteBookingValidator()
    {
        RuleFor(x => x.BookingId).NotEmpty();
    }
}

public class CompleteBookingHandler : IRequestHandler<CompleteBookingCommand, BookingDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _current;
    private readonly IMapper _mapper;

    public CompleteBookingHandler(IUnitOfWork uow, ICurrentUserService current, IMapper mapper)
    {
        _uow = uow;
        _current = current;
        _mapper = mapper;
    }

    public async Task<BookingDto> Handle(CompleteBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _uow.Bookings.GetByIdAsync(request.BookingId) ?? throw new NotFoundException("Booking not found");
        var listing = await _uow.Listings.GetByIdAsync(booking.ListingId) ?? throw new NotFoundException("Listing not found");
        var vendor = await _uow.Vendors.GetByIdAsync(listing.VendorId) ?? throw new NotFoundException("Vendor not found");

        if (vendor.UserId != _current.UserId) throw new ForbiddenException("Only vendor can complete booking");
        if (booking.Status != BookingStatus.Confirmed) throw new ConflictException("Only confirmed bookings can be completed");
        if (booking.Slot.EndTime > DateTime.UtcNow) throw new ConflictException("Cannot complete booking before slot end time");

        booking.Status = BookingStatus.Completed;
        booking.CompletedAt = DateTime.UtcNow;
        _uow.Bookings.Update(booking);

        // increment listing total bookings
        listing.TotalBookings += 1;
        _uow.Listings.Update(listing);

        await _uow.SaveChangesAsync();

        return _mapper.Map<BookingDto>(booking);
    }
}
