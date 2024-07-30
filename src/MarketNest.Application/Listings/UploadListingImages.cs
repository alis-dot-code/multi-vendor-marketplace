using FluentValidation;
using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Enums;
using MarketNest.Domain.Interfaces.Repositories;
using AutoMapper;
using MarketNest.Application.Common.Models;
using System.IO;

namespace MarketNest.Application.Listings;

public record UploadListingImagesCommand(Guid ListingId, List<FileUpload> Files) : IRequest<List<ListingImageDto>>;

public class UploadListingImagesValidator : AbstractValidator<UploadListingImagesCommand>
{
    public UploadListingImagesValidator()
    {
        RuleFor(x => x.ListingId).NotEmpty();
    }
}

public class UploadListingImagesHandler : IRequestHandler<UploadListingImagesCommand, List<ListingImageDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICloudinaryService _cloud;
    private readonly ICurrentUserService _current;
    private readonly IMapper _mapper;

    public UploadListingImagesHandler(IUnitOfWork uow, ICloudinaryService cloud, ICurrentUserService current, IMapper mapper)
    {
        _uow = uow;
        _cloud = cloud;
        _current = current;
        _mapper = mapper;
    }

    public async Task<List<ListingImageDto>> Handle(UploadListingImagesCommand request, CancellationToken cancellationToken)
    {
        var listing = await _uow.Listings.GetByIdAsync(request.ListingId) ?? throw new NotFoundException("Listing not found");
        var vendor = await _uow.Vendors.GetByIdAsync(listing.VendorId) ?? throw new NotFoundException("Vendor not found");
        if (vendor.UserId != _current.UserId) throw new ForbiddenException("Only owner can upload images");

        if (request.Files == null || !request.Files.Any()) throw new BadRequestException("No files provided");
        if (request.Files.Count > 10) throw new BadRequestException("Max 10 files allowed");

        var created = new List<ListingImage>();
        foreach (var file in request.Files)
        {
            if (file.Content == null) continue;
            if (file.Content.Length > 5 * 1024 * 1024) { /* best-effort - stream may not support Length */ }

            using var ms = new MemoryStream();
            await file.Content.CopyToAsync(ms);
            ms.Position = 0;
            var folder = $"listings/{listing.Id}";
            var (url, publicId) = await _cloud.UploadImageAsync(ms, folder);

            var img = new ListingImage
            {
                ListingId = listing.Id,
                Url = url,
                PublicId = publicId,
                IsPrimary = !listing.Images.Any() && !created.Any(),
                SortOrder = listing.Images.Count + created.Count
            };
            listing.Images.Add(img);
            created.Add(img);
        }

        await _uow.SaveChangesAsync();

        return created.Select(i => _mapper.Map<ListingImageDto>(i)).ToList();
    }
}
