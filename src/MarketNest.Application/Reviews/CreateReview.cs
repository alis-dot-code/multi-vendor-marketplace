using System;
using System.Threading;
using System.Threading.Tasks;
using MarketNest.Application.Common.Models;
using MarketNest.Domain.Entities;
using MarketNest.Domain.Enums;
using MarketNest.Domain.Interfaces.Repositories;
using MarketNest.Application.Common.Interfaces;
using MediatR;

namespace MarketNest.Application.Reviews
{
    public static class CreateReview
    {
        public record Command(Guid BookingId, int Rating, string Comment) : IRequest<Result<Review>>;

        public class Handler : IRequestHandler<Command, Result<Review>>
        {
            private readonly IUnitOfWork _uow;
            private readonly ICurrentUserService _currentUser;
            private readonly IEmailService _emailService;

            public Handler(IUnitOfWork uow, ICurrentUserService currentUser, IEmailService emailService)
            {
                _uow = uow;
                _currentUser = currentUser;
                _emailService = emailService;
            }

            public async Task<Result<Review>> Handle(Command request, CancellationToken cancellationToken)
            {
                var booking = await _uow.Bookings.GetByIdAsync(request.BookingId);
                if (booking == null) return Result<Review>.Failure("Booking not found");
                if (booking.BuyerId != _currentUser.UserId) return Result<Review>.Failure("Forbidden");
                if (booking.Status != BookingStatus.Completed) return Result<Review>.Failure("Booking not completed");
                var exists = await _uow.Reviews.ExistsByBookingAsync(request.BookingId);
                if (exists) return Result<Review>.Failure("Review already exists for this booking");

                var review = new Review
                {
                    BookingId = request.BookingId,
                    BuyerId = _currentUser.UserId,
                    ListingId = booking.ListingId,
                    VendorId = booking.VendorId,
                    Rating = request.Rating,
                    Comment = request.Comment,
                    IsVisible = true
                };

                await _uow.Reviews.AddAsync(review);

                // recalc listing and vendor ratings (simple aggregate)
                var listingReviews = (await _uow.Reviews.FindAsync(r => r.ListingId == review.ListingId && r.IsVisible && !r.AdminHidden)).ToList();
                listingReviews.Add(review);
                var avg = listingReviews.Any() ? listingReviews.Average(r => r.Rating) : 0.0;

                var listing = await _uow.Listings.GetByIdAsync(review.ListingId);
                if (listing != null)
                {
                    listing.AvgRating = (decimal)avg;
                    listing.TotalReviews = listingReviews.Count;
                    _uow.Listings.Update(listing);
                }

                var vendorReviews = (await _uow.Reviews.FindAsync(r => r.VendorId == review.VendorId && r.IsVisible && !r.AdminHidden)).ToList();
                vendorReviews.Add(review);
                var vavg = vendorReviews.Any() ? vendorReviews.Average(r => r.Rating) : 0.0;
                var vendor = await _uow.Vendors.GetByIdAsync(review.VendorId);
                if (vendor != null)
                {
                    vendor.AvgRating = (decimal)vavg;
                    vendor.TotalReviews = vendorReviews.Count;
                    _uow.Vendors.Update(vendor);

                    // send email and DB notification
                    await _emailService.SendEmailAsync(vendor.User.Email, "You received a new review", $"Rating: {review.Rating}<br/>{review.Comment}");
                    await _uow.Notifications.AddAsync(new Notification
                    {
                        UserId = vendor.UserId,
                        Type = NotificationType.ReviewReceived,
                        Title = "New review received",
                        Message = $"You received a new review for booking {booking.BookingNumber}",
                        IsRead = false
                    });
                }

                await _uow.SaveChangesAsync();

                return Result<Review>.Success(review);
            }
        }
    }
}
