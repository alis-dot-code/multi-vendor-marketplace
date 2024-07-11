using MarketNest.Domain.Entities;
using MarketNest.Application.Common.DTOs;
using AutoMapper;

namespace MarketNest.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<User, UserDto>();
        
        // Vendor
        CreateMap<Vendor, VendorDto>();
        CreateMap<Vendor, VendorDashboardDto>()
            .ForMember(dest => dest.TotalEarningsCents, opt => opt.Ignore())
            .ForMember(dest => dest.ThisMonthEarningsCents, opt => opt.Ignore());
        
        // Listing
        CreateMap<Listing, ListingDto>();
        CreateMap<Listing, ListingDetailDto>();
        CreateMap<Listing, ListingSearchResultDto>();
        
        // Booking
        CreateMap<Booking, BookingDto>();
        CreateMap<AvailabilitySlot, AvailabilitySlotDto>();
        
        // Payment
        CreateMap<Payment, PaymentDto>();
        
        // Review
        CreateMap<Review, ReviewDto>();
        CreateMap<ReviewReply, ReviewReplyDto>();
        
        // Category
        CreateMap<Category, CategoryDto>();
        
        // Dispute
        CreateMap<Dispute, DisputeDto>();
        
        // Notification
        CreateMap<Notification, NotificationDto>();
        
        // PlatformSetting
        CreateMap<PlatformSetting, PlatformSettingDto>();
    }
}
