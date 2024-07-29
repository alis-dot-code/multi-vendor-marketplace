using FluentValidation;
using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Interfaces.Repositories;
using AutoMapper;

namespace MarketNest.Application.Listings.Availability;

public record DeleteAvailabilitySlotCommand(Guid SlotId) : IRequest<Unit>;

public class DeleteAvailabilitySlotValidator : AbstractValidator<DeleteAvailabilitySlotCommand>
{
    public DeleteAvailabilitySlotValidator()
    {
        RuleFor(x => x.SlotId).NotEmpty();
    }
}

public class DeleteAvailabilitySlotHandler : IRequestHandler<DeleteAvailabilitySlotCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _current;

    public DeleteAvailabilitySlotHandler(IUnitOfWork uow, ICurrentUserService current)
    {
        _uow = uow;
        _current = current;
    }

    public async Task<Unit> Handle(DeleteAvailabilitySlotCommand request, CancellationToken cancellationToken)
    {
        var listings = (await _uow.Listings.FindAsync(l => l.AvailabilitySlots.Any(s => s.Id == request.SlotId))).ToList();
        var listing = listings.FirstOrDefault() ?? throw new NotFoundException("Slot not found");
        var slot = listing.AvailabilitySlots.FirstOrDefault(s => s.Id == request.SlotId) ?? throw new NotFoundException("Slot not found");

        if (slot.IsBooked) throw new ConflictException("Cannot delete booked slot");

        var vendor = await _uow.Vendors.GetByIdAsync(listing.VendorId) ?? throw new NotFoundException("Vendor not found");
        if (vendor.UserId != _current.UserId) throw new ForbiddenException("Only owner can delete slot");

        listing.AvailabilitySlots.Remove(slot);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}
