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

public record CreateListingCommand(string Title, string Description, string? ShortDescription, int PriceCents, int DurationMinutes, Guid CategoryId, bool IsVirtual, string? VirtualLink) : IRequest<ListingDto>;

public class CreateListingValidator : AbstractValidator<CreateListingCommand>
{
    public CreateListingValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.PriceCents).GreaterThan(0);
        RuleFor(x => x.DurationMinutes).GreaterThan(0);
    }
}

public class CreateListingHandler : IRequestHandler<CreateListingCommand, ListingDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _current;
    private readonly IMapper _mapper;

    public CreateListingHandler(IUnitOfWork uow, ICurrentUserService current, IMapper mapper)
    {
        _uow = uow;
        _current = current;
        _mapper = mapper;
    }

    public async Task<ListingDto> Handle(CreateListingCommand request, CancellationToken cancellationToken)
    {
        if (!_current.IsAuthenticated) throw new ForbiddenException("Authentication required");

        var vendor = (await _uow.Vendors.FindAsync(v => v.UserId == _current.UserId && v.Status == VendorStatus.Approved)).FirstOrDefault();
        if (vendor == null) throw new ForbiddenException("Approved vendor account required");

        var slugBase = SlugUtil.GenerateSlug(request.Title);
        var slug = slugBase;
        var idx = 1;
        while ((await _uow.Listings.FindAsync(l => l.VendorId == vendor.Id && l.Slug == slug)).Any())
        {
            slug = slugBase + "-" + idx++; 
        }

        var listing = new Listing
        {
            VendorId = vendor.Id,
            CategoryId = request.CategoryId,
            Title = request.Title,
            Slug = slug,
            Description = request.Description,
            ShortDescription = request.ShortDescription,
            PriceCents = request.PriceCents,
            DurationMinutes = request.DurationMinutes,
            IsVirtual = request.IsVirtual,
            VirtualLink = request.VirtualLink,
            Status = ListingStatus.Draft
        };

        await _uow.Listings.AddAsync(listing);
        await _uow.SaveChangesAsync();

        return _mapper.Map<ListingDto>(listing);
    }
}
