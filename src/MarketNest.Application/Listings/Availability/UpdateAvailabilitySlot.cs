using FluentValidation;
using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Interfaces.Repositories;
using AutoMapper;

namespace MarketNest.Application.Listings.Availability;

public record UpdateAvailabilitySlotCommand(Guid SlotId, bool? IsBlocked) : IRequest<AvailabilitySlotDto>;

public class UpdateAvailabilitySlotValidator : AbstractValidator<UpdateAvailabilitySlotCommand>
{
    public UpdateAvailabilitySlotValidator()
    {
        RuleFor(x => x.SlotId).NotEmpty();
    }
}

public class UpdateAvailabilitySlotHandler : IRequestHandler<UpdateAvailabilitySlotCommand, AvailabilitySlotDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _current;
    private readonly IMapper _mapper;

    public UpdateAvailabilitySlotHandler(IUnitOfWork uow, ICurrentUserService current, IMapper mapper)
    {
        _uow = uow;
        _current = current;
        _mapper = mapper;
    }

    public async Task<AvailabilitySlotDto> Handle(UpdateAvailabilitySlotCommand request, CancellationToken cancellationToken)
    {
        var listings = (await _uow.Listings.FindAsync(l => l.AvailabilitySlots.Any(s => s.Id == request.SlotId))).ToList();
        var listing = listings.FirstOrDefault();
        if (listing == null) throw new NotFoundException("Slot not found");

        var found = listing.AvailabilitySlots.FirstOrDefault(s => s.Id == request.SlotId);
        if (found == null) throw new NotFoundException("Slot not found");

        var vendor = await _uow.Vendors.GetByIdAsync(listing.VendorId) ?? throw new NotFoundException("Vendor not found");
        if (vendor.UserId != _current.UserId) throw new ForbiddenException("Only owner can update slot");

        if (request.IsBlocked.HasValue) found.IsBlocked = request.IsBlocked.Value;
        await _uow.SaveChangesAsync();

        return _mapper.Map<AvailabilitySlotDto>(found);
    }
}
