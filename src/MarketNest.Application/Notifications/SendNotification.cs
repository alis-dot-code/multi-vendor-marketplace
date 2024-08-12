using System.Threading;
using System.Threading.Tasks;
using MarketNest.Application.Common.Models;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Interfaces.Repositories;
using MarketNest.Application.Common.Interfaces;
using MediatR;

namespace MarketNest.Application.Notifications
{
    public static class SendNotification
    {
        public record Command(Guid UserId, Domain.Enums.NotificationType Type, string Title, string Message, string? Data = null) : IRequest<Result<bool>>;

        public class Handler : IRequestHandler<Command, Result<bool>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IEmailService _emailService;

            public Handler(IUnitOfWork uow, IEmailService emailService)
            {
                _uow = uow;
                _emailService = emailService;
            }

            public async Task<Result<bool>> Handle(Command request, CancellationToken cancellationToken)
            {
                var notification = new Notification
                {
                    UserId = request.UserId,
                    Type = request.Type,
                    Title = request.Title,
                    Message = request.Message,
                    Data = request.Data,
                    IsRead = false
                };
                await _uow.Notifications.AddAsync(notification);
                await _uow.SaveChangesAsync();

                var user = await _uow.Users.GetByIdAsync(request.UserId);
                if (user != null)
                {
                    await _emailService.SendEmailAsync(user.Email, request.Title, request.Message);
                }

                return Result<bool>.Success(true);
            }
        }
    }
}
