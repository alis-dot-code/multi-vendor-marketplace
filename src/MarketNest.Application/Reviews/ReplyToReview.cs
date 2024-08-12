using System;
using System.Threading;
using System.Threading.Tasks;
using MarketNest.Application.Common.Models;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Interfaces.Repositories;
using MarketNest.Application.Common.Interfaces;
using MediatR;

namespace MarketNest.Application.Reviews
{
    public static class ReplyToReview
    {
        public record Command(Guid ReviewId, string Reply) : IRequest<Result<ReviewReply>>;

        public class Handler : IRequestHandler<Command, Result<ReviewReply>>
        {
            private readonly IUnitOfWork _uow;
            private readonly ICurrentUserService _currentUser;

            public Handler(IUnitOfWork uow, ICurrentUserService currentUser)
            {
                _uow = uow;
                _currentUser = currentUser;
            }

            public async Task<Result<ReviewReply>> Handle(Command request, CancellationToken cancellationToken)
            {
                var review = await _uow.Reviews.GetByIdAsync(request.ReviewId);
                if (review == null) return Result<ReviewReply>.Failure("Review not found");

                // verify vendor owns the listing
                var listing = await _uow.Listings.GetByIdAsync(review.ListingId);
                if (listing == null) return Result<ReviewReply>.Failure("Listing not found");
                if (listing.VendorId != _currentUser.UserId) return Result<ReviewReply>.Failure("Forbidden");

                if (review.Reply != null) return Result<ReviewReply>.Failure("Reply already exists");

                var reply = new ReviewReply
                {
                    ReviewId = review.Id,
                    VendorId = _currentUser.UserId,
                    Comment = request.Reply
                };

                // attach reply to review and update
                review.Reply = reply;
                _uow.Reviews.Update(review);
                await _uow.SaveChangesAsync();

                return Result<ReviewReply>.Success(reply);
            }
        }
    }
}
