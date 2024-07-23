using AutoMapper;
using FluentValidation;
using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Interfaces.Repositories;
using MarketNest.Application.Common.Utils;

namespace MarketNest.Application.Vendors.Commands;

public record ApplyVendorCommand(string BusinessName, string Description, string City, string Country, string Phone) : IRequest<VendorDto>;

public class ApplyVendorValidator : AbstractValidator<ApplyVendorCommand>
{
    public ApplyVendorValidator()
    {
        RuleFor(x => x.BusinessName).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}

public class ApplyVendorHandler : IRequestHandler<ApplyVendorCommand, VendorDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public ApplyVendorHandler(IUnitOfWork uow, IMapper mapper, ICurrentUserService currentUser)
    {
        _uow = uow;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<VendorDto> Handle(ApplyVendorCommand request, CancellationToken cancellationToken)
    {
        var existing = await _uow.Vendors.FindByUserIdAsync(_currentUser.UserId);
        if (existing != null) throw new ConflictException("User already has a vendor application");

        // generate slug
        var slugBase = SlugUtil.GenerateSlug(request.BusinessName);
        var slug = slugBase;
        var i = 1;
        while (await _uow.Vendors.FindBySlugAsync(slug) != null)
        {
            slug = slugBase + "-" + i++;
        }

        var vendor = new Vendor
        {
            UserId = _currentUser.UserId,
            BusinessName = request.BusinessName,
            Description = request.Description,
            City = request.City,
            Country = request.Country,
            Phone = request.Phone,
            Slug = slug,
            Status = MarketNest.Domain.Enums.VendorStatus.Pending
        };

        await _uow.Vendors.AddAsync(vendor);
        await _uow.SaveChangesAsync();

        return _mapper.Map<VendorDto>(vendor);
    }
}
