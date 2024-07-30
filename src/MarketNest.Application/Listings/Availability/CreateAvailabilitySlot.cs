using FluentValidation;
using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Interfaces.Repositories;
using AutoMapper;

namespace MarketNest.Application.Listings.Availability;

public record CreateAvailabilitySlotCommand(Guid ListingId, DateTime StartTime, DateTime EndTime) : IRequest<AvailabilitySlotDto>;

public class CreateAvailabilitySlotValidator : AbstractValidator<CreateAvailabilitySlotCommand>
{
    public CreateAvailabilitySlotValidator()
    {
        RuleFor(x => x.ListingId).NotEmpty();
        RuleFor(x => x.StartTime).LessThan(x => x.EndTime);
    }
}

public class CreateAvailabilitySlotHandler : IRequestHandler<CreateAvailabilitySlotCommand, AvailabilitySlotDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _current;
    private readonly IMapper _mapper;

    public CreateAvailabilitySlotHandler(IUnitOfWork uow, ICurrentUserService current, IMapper mapper)
    {
        _uow = uow;
        _current = current;
        _mapper = mapper;
    }

    public async Task<AvailabilitySlotDto> Handle(CreateAvailabilitySlotCommand request, CancellationToken cancellationToken)
    {
        var listing = await _uow.Listings.GetByIdAsync(request.ListingId) ?? throw new NotFoundException("Listing not found");
        var vendor = await _uow.Vendors.GetByIdAsync(listing.VendorId) ?? throw new NotFoundException("Vendor not found");
        if (vendor.UserId != _current.UserId) throw new ForbiddenException("Only owner can create slots");

        var slot = new AvailabilitySlot
        {
            ListingId = listing.Id,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            IsBooked = false,
            IsBlocked = false
        };
        listing.AvailabilitySlots.Add(slot);
        await _uow.SaveChangesAsync();

        return _mapper.Map<AvailabilitySlotDto>(slot);
    }
}
