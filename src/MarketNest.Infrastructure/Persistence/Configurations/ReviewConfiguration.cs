using MarketNest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketNest.Infrastructure.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("reviews");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedOnAdd();
        
        builder.HasIndex(r => r.BookingId).IsUnique();
        
        builder.Property(r => r.Rating).IsRequired();
        builder.HasCheckConstraint("CK_Reviews_Rating_Range", "rating >= 1 AND rating <= 5");
        
        builder.Property(r => r.Title).HasMaxLength(200);
        builder.Property(r => r.Comment).IsRequired().HasMaxLength(2000);
        builder.Property(r => r.IsVisible);
        builder.Property(r => r.IsFlagged);
        builder.Property(r => r.AdminHidden);
        
        builder.HasIndex(r => new { r.ListingId, r.IsVisible });
        
        builder.HasOne(r => r.Booking).WithOne(b => b.Review).HasForeignKey<Review>(r => r.BookingId);
        builder.HasOne(r => r.Buyer).WithMany(u => u.Reviews).HasForeignKey(r => r.BuyerId);
        builder.HasOne(r => r.Listing).WithMany(l => l.Reviews).HasForeignKey(r => r.ListingId);
        builder.HasOne(r => r.Vendor).WithMany(v => v.Reviews).HasForeignKey(r => r.VendorId);
        builder.HasOne(r => r.Reply).WithOne(re => re.Review).HasForeignKey<ReviewReply>(re => re.ReviewId);
    }
}
