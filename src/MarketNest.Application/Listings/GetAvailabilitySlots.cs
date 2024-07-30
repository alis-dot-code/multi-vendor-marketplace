using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Interfaces.Repositories;
using AutoMapper;

namespace MarketNest.Application.Listings;

public record GetAvailabilitySlotsQuery(Guid ListingId, DateTime? From, DateTime? To) : IRequest<List<AvailabilitySlotDto>>;

public class GetAvailabilitySlotsHandler : IRequestHandler<GetAvailabilitySlotsQuery, List<AvailabilitySlotDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetAvailabilitySlotsHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<List<AvailabilitySlotDto>> Handle(GetAvailabilitySlotsQuery request, CancellationToken cancellationToken)
    {
        var listing = await _uow.Listings.GetByIdAsync(request.ListingId) ?? throw new NotFoundException("Listing not found");
        var slots = listing.AvailabilitySlots.AsEnumerable();
        if (request.From.HasValue) slots = slots.Where(s => s.EndTime >= request.From.Value);
        if (request.To.HasValue) slots = slots.Where(s => s.StartTime <= request.To.Value);

        return slots.Select(s => _mapper.Map<AvailabilitySlotDto>(s)).ToList();
    }
}
