using FluentValidation;
using AutoMapper;
using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Interfaces.Repositories;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Application.Common.Utils;

namespace MarketNest.Application.Vendors.Commands;

public record UpdateVendorProfileCommand(Guid VendorId, string BusinessName, string Description, string? City, string? Country, string? Phone) : IRequest<VendorDto>;

public class UpdateVendorProfileValidator : AbstractValidator<UpdateVendorProfileCommand>
{
    public UpdateVendorProfileValidator()
    {
        RuleFor(x => x.VendorId).NotEmpty();
        RuleFor(x => x.BusinessName).NotEmpty();
    }
}

public class UpdateVendorProfileHandler : IRequestHandler<UpdateVendorProfileCommand, VendorDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public UpdateVendorProfileHandler(IUnitOfWork uow, IMapper mapper, ICurrentUserService currentUser)
    {
        _uow = uow;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<VendorDto> Handle(UpdateVendorProfileCommand request, CancellationToken cancellationToken)
    {
        var vendor = await _uow.Vendors.GetByIdAsync(request.VendorId);
        if (vendor == null) throw new NotFoundException("Vendor not found");
        if (vendor.UserId != _currentUser.UserId) throw new ForbiddenException("Not owner");

        if (!string.Equals(vendor.BusinessName, request.BusinessName, StringComparison.OrdinalIgnoreCase))
        {
            var slugBase = SlugUtil.GenerateSlug(request.BusinessName);
            var slug = slugBase; var i = 1;
            while (await _uow.Vendors.FindBySlugAsync(slug) != null)
            {
                slug = slugBase + "-" + i++;
            }
            vendor.Slug = slug;
        }

        vendor.BusinessName = request.BusinessName;
        vendor.Description = request.Description;
        vendor.City = request.City;
        vendor.Country = request.Country;
        vendor.Phone = request.Phone;

        _uow.Vendors.Update(vendor);
        await _uow.SaveChangesAsync();

        return _mapper.Map<VendorDto>(vendor);
    }
}
