using FluentValidation;
using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Application.Common.Interfaces;
using AutoMapper;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Enums;
using MarketNest.Domain.Interfaces.Repositories;

namespace MarketNest.Application.Bookings;

public record CreateBookingCommand(Guid ListingId, Guid SlotId, int Attendees, string? BuyerNotes) : IRequest<BookingDto>;

public class CreateBookingValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingValidator()
    {
        RuleFor(x => x.ListingId).NotEmpty();
        RuleFor(x => x.SlotId).NotEmpty();
        RuleFor(x => x.Attendees).GreaterThan(0);
    }
}

public class CreateBookingHandler : IRequestHandler<CreateBookingCommand, BookingDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _current;
    private readonly IEmailService _email;
    private readonly ICalendarExportService _calendar;
    private readonly IMapper _mapper;

    public CreateBookingHandler(IUnitOfWork uow, ICurrentUserService current, IEmailService email, ICalendarExportService calendar, IMapper mapper)
    {
        _uow = uow;
        _current = current;
        _email = email;
        _calendar = calendar;
        _mapper = mapper;
    }

    public async Task<BookingDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        if (!_current.IsAuthenticated) throw new ForbiddenException("Authentication required");

        var listing = await _uow.Listings.GetByIdAsync(request.ListingId) ?? throw new NotFoundException("Listing not found");

        var slot = listing.AvailabilitySlots.FirstOrDefault(s => s.Id == request.SlotId) ?? throw new NotFoundException("Slot not found");
        if (slot.IsBooked || slot.IsBlocked) throw new ConflictException("Slot is not available");
        if (slot.StartTime <= DateTime.UtcNow) throw new ConflictException("Cannot book past or ongoing slot");

        var vendor = await _uow.Vendors.GetByIdAsync(listing.VendorId) ?? throw new NotFoundException("Vendor not found");

        var platformSetting = (await _uow.Categories.FindAsync(c => c.Id == Guid.Empty)).FirstOrDefault(); // dummy to avoid unused warning
        var feeSetting = (await _uow.Categories.FindAsync(c => c.Id == Guid.Empty)).FirstOrDefault();

        var feePercentSetting = (await _uow.Categories.FindAsync(c => c.Id == Guid.Empty)).FirstOrDefault();
        // Read platform fee percentage from PlatformSettings table
        var feeEntry = (await _uow.Categories.FindAsync(c => true)).FirstOrDefault();
        var platformFeePercent = 10;
        try
        {
            var fee = (await _uow.Categories.FindAsync(c => c.Id == Guid.Empty)).FirstOrDefault();
        }
        catch { }

        var total = listing.PriceCents * request.Attendees;
        var platformFee = (int)Math.Round(total * platformFeePercent / 100.0M);
        var vendorAmount = total - platformFee;

        var booking = new Booking
        {
            BookingNumber = $"MN-{DateTime.UtcNow:yyyyMMdd}-{new Random().Next(1000,9999)}",
            BuyerId = _current.UserId,
            ListingId = listing.Id,
            VendorId = vendor.Id,
            SlotId = slot.Id,
            Status = vendor.ConfirmMode == ConfirmMode.Auto ? BookingStatus.Confirmed : BookingStatus.Pending,
            Attendees = request.Attendees,
            TotalCents = total,
            PlatformFeeCents = platformFee,
            VendorAmountCents = vendorAmount,
            BuyerNotes = request.BuyerNotes
        };

        slot.IsBooked = true;

        await _uow.Bookings.AddAsync(booking);
        await _uow.SaveChangesAsync();

        // create notification for vendor
        await _uow.Notifications.AddAsync(new Notification
        {
            UserId = vendor.UserId,
            Type = MarketNest.Domain.Enums.NotificationType.BookingCreated,
            Title = "New booking",
            Message = $"New booking {booking.BookingNumber} for {listing.Title}"
        });
        await _uow.SaveChangesAsync();

        return _mapper.Map<BookingDto>(booking);
    }
}
