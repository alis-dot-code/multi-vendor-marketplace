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

public record RejectVendorCommand(Guid VendorId, string Reason) : IRequest<VendorDto>;

public class RejectVendorValidator : AbstractValidator<RejectVendorCommand>
{
    public RejectVendorValidator()
    {
        RuleFor(x => x.VendorId).NotEmpty();
    }
}

public class RejectVendorHandler : IRequestHandler<RejectVendorCommand, VendorDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public RejectVendorHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<VendorDto> Handle(RejectVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = await _uow.Vendors.GetByIdAsync(request.VendorId);
        if (vendor == null) throw new NotFoundException("Vendor not found");

        vendor.Status = VendorStatus.Rejected;
        vendor.AdminNotes = request.Reason;
        _uow.Vendors.Update(vendor);

        var note = new Notification
        {
            UserId = vendor.UserId,
            Type = MarketNest.Domain.Enums.NotificationType.VendorRejected,
            Title = "Vendor Rejected",
            Message = request.Reason,
            CreatedAt = DateTime.UtcNow
        };
        await _uow.Notifications.AddAsync(note);

        await _uow.SaveChangesAsync();

        return _mapper.Map<VendorDto>(vendor);
    }
}
