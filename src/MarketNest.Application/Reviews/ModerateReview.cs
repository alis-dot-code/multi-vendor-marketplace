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
    public static class ModerateReview
    {
        public record Command(Guid ReviewId, bool Hide) : IRequest<Result<bool>>;

        public class Handler : IRequestHandler<Command, Result<bool>>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Result<bool>> Handle(Command request, CancellationToken cancellationToken)
            {
                var review = await _uow.Reviews.GetByIdAsync(request.ReviewId);
                if (review == null) return Result<bool>.Failure("Review not found");

                review.AdminHidden = request.Hide;
                _uow.Reviews.Update(review);

                // recalc listing
                var listingReviews = (await _uow.Reviews.FindAsync(r => r.ListingId == review.ListingId && r.IsVisible && !r.AdminHidden)).ToList();
                var avg = listingReviews.Any() ? listingReviews.Average(r => r.Rating) : 0.0;
                var listing = await _uow.Listings.GetByIdAsync(review.ListingId);
                if (listing != null)
                {
                    listing.AvgRating = (decimal)avg;
                    listing.TotalReviews = listingReviews.Count;
                    _uow.Listings.Update(listing);
                }

                var vendorReviews = (await _uow.Reviews.FindAsync(r => r.VendorId == review.VendorId && r.IsVisible && !r.AdminHidden)).ToList();
                var vavg = vendorReviews.Any() ? vendorReviews.Average(r => r.Rating) : 0.0;
                var vendor = await _uow.Vendors.GetByIdAsync(review.VendorId);
                if (vendor != null)
                {
                    vendor.AvgRating = (decimal)vavg;
                    vendor.TotalReviews = vendorReviews.Count;
                    _uow.Vendors.Update(vendor);
                }

                await _uow.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
        }
    }
}
