using FluentValidation;
using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Enums;
using MarketNest.Domain.Interfaces.Repositories;
using AutoMapper;

namespace MarketNest.Application.Listings;

public record DeleteListingCommand(Guid ListingId) : IRequest<Unit>;

public class DeleteListingValidator : AbstractValidator<DeleteListingCommand>
{
    public DeleteListingValidator()
    {
        RuleFor(x => x.ListingId).NotEmpty();
    }
}

public class DeleteListingHandler : IRequestHandler<DeleteListingCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _current;

    public DeleteListingHandler(IUnitOfWork uow, ICurrentUserService current)
    {
        _uow = uow;
        _current = current;
    }

    public async Task<Unit> Handle(DeleteListingCommand request, CancellationToken cancellationToken)
    {
        var listing = await _uow.Listings.GetByIdAsync(request.ListingId) ?? throw new NotFoundException("Listing not found");
        var vendor = await _uow.Vendors.GetByIdAsync(listing.VendorId) ?? throw new NotFoundException("Vendor not found");
        if (vendor.UserId != _current.UserId) throw new ForbiddenException("Only owner can delete listing");

        var activeBookings = (await _uow.Bookings.FindAsync(b => b.ListingId == listing.Id && (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Confirmed))).Any();
        if (activeBookings) throw new ConflictException("Cannot delete listing with active bookings");

        listing.Status = ListingStatus.Archived;
        _uow.Listings.Update(listing);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}
