using FluentValidation;
using AutoMapper;
using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Enums;
using MarketNest.Domain.Interfaces.Repositories;
using MarketNest.Application.Common.Interfaces;

namespace MarketNest.Application.Vendors.Commands;

public record ApproveVendorCommand(Guid VendorId, string AdminNotes) : IRequest<VendorDto>;

public class ApproveVendorValidator : AbstractValidator<ApproveVendorCommand>
{
    public ApproveVendorValidator()
    {
        RuleFor(x => x.VendorId).NotEmpty();
    }
}

public class ApproveVendorHandler : IRequestHandler<ApproveVendorCommand, VendorDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ApproveVendorHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<VendorDto> Handle(ApproveVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = await _uow.Vendors.GetByIdAsync(request.VendorId);
        if (vendor == null) throw new NotFoundException("Vendor not found");

        vendor.Status = VendorStatus.Approved;
        vendor.ApprovedAt = DateTime.UtcNow;
        vendor.AdminNotes = request.AdminNotes;

        // update user role
        var user = await _uow.Users.GetByIdAsync(vendor.UserId);
        if (user != null)
        {
            user.Role = Domain.Enums.UserRole.Vendor;
            _uow.Users.Update(user);
        }

        _uow.Vendors.Update(vendor);

        // notification
        var note = new Notification
        {
            UserId = vendor.UserId,
            Type = MarketNest.Domain.Enums.NotificationType.VendorApproved,
            Title = "Vendor Approved",
            Message = "Your vendor application has been approved",
            CreatedAt = DateTime.UtcNow
        };
        await _uow.Notifications.AddAsync(note);

        await _uow.SaveChangesAsync();

        return _mapper.Map<VendorDto>(vendor);
    }
}
