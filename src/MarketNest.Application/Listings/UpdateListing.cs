using FluentValidation;
using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Enums;
using MarketNest.Domain.Interfaces.Repositories;
using AutoMapper;
using MarketNest.Application.Common.Utils;

namespace MarketNest.Application.Listings;

public record UpdateListingCommand(Guid ListingId, string Title, string Description, string? ShortDescription, int PriceCents, int DurationMinutes, Guid CategoryId, bool IsVirtual, string? VirtualLink, ListingStatus Status) : IRequest<ListingDto>;

public class UpdateListingValidator : AbstractValidator<UpdateListingCommand>
{
    public UpdateListingValidator()
    {
        RuleFor(x => x.ListingId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
    }
}

public class UpdateListingHandler : IRequestHandler<UpdateListingCommand, ListingDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _current;
    private readonly IMapper _mapper;

    public UpdateListingHandler(IUnitOfWork uow, ICurrentUserService current, IMapper mapper)
    {
        _uow = uow;
        _current = current;
        _mapper = mapper;
    }

    public async Task<ListingDto> Handle(UpdateListingCommand request, CancellationToken cancellationToken)
    {
        var listing = await _uow.Listings.GetByIdAsync(request.ListingId) ?? throw new NotFoundException("Listing not found");

        var vendor = await _uow.Vendors.GetByIdAsync(listing.VendorId) ?? throw new NotFoundException("Vendor not found");
        if (vendor.UserId != _current.UserId) throw new ForbiddenException("Only listing owner can update");

        if (!string.Equals(listing.Title, request.Title, StringComparison.OrdinalIgnoreCase))
        {
            var slugBase = SlugUtil.GenerateSlug(request.Title);
            var slug = slugBase; var idx = 1;
            while ((await _uow.Listings.FindAsync(l => l.VendorId == vendor.Id && l.Slug == slug && l.Id != listing.Id)).Any())
            {
                slug = slugBase + "-" + idx++;
            }
            listing.Slug = slug;
        }

        listing.Title = request.Title;
        listing.Description = request.Description;
        listing.ShortDescription = request.ShortDescription;
        listing.PriceCents = request.PriceCents;
        listing.DurationMinutes = request.DurationMinutes;
        listing.CategoryId = request.CategoryId;
        listing.IsVirtual = request.IsVirtual;
        listing.VirtualLink = request.VirtualLink;
        listing.Status = request.Status;

        _uow.Listings.Update(listing);
        await _uow.SaveChangesAsync();

        return _mapper.Map<ListingDto>(listing);
    }
}
