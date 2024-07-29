using FluentValidation;
using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Interfaces.Repositories;

namespace MarketNest.Application.Listings;

public record DeleteListingImageCommand(Guid ListingId, Guid ImageId) : IRequest<Unit>;

public class DeleteListingImageValidator : AbstractValidator<DeleteListingImageCommand>
{
    public DeleteListingImageValidator()
    {
        RuleFor(x => x.ListingId).NotEmpty();
        RuleFor(x => x.ImageId).NotEmpty();
    }
}

public class DeleteListingImageHandler : IRequestHandler<DeleteListingImageCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly ICloudinaryService _cloud;
    private readonly ICurrentUserService _current;

    public DeleteListingImageHandler(IUnitOfWork uow, ICloudinaryService cloud, ICurrentUserService current)
    {
        _uow = uow;
        _cloud = cloud;
        _current = current;
    }

    public async Task<Unit> Handle(DeleteListingImageCommand request, CancellationToken cancellationToken)
    {
        var listing = await _uow.Listings.GetByIdAsync(request.ListingId) ?? throw new NotFoundException("Listing not found");
        var image = listing.Images.FirstOrDefault(i => i.Id == request.ImageId) ?? throw new NotFoundException("Image not found");
        var vendor = await _uow.Vendors.GetByIdAsync(listing.VendorId) ?? throw new NotFoundException("Vendor not found");
        if (vendor.UserId != _current.UserId) throw new ForbiddenException("Only owner can delete image");

        if (!string.IsNullOrWhiteSpace(image.PublicId)) await _cloud.DeleteImageAsync(image.PublicId);
        listing.Images.Remove(image);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}
