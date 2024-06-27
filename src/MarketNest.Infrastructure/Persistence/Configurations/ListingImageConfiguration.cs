using MarketNest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketNest.Infrastructure.Persistence.Configurations;

public class ListingImageConfiguration : IEntityTypeConfiguration<ListingImage>
{
    public void Configure(EntityTypeBuilder<ListingImage> builder)
    {
        builder.ToTable("listing_images");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).ValueGeneratedOnAdd();
        
        builder.Property(i => i.Url).HasMaxLength(512).IsRequired();
        builder.Property(i => i.PublicId).HasMaxLength(256).IsRequired();
        builder.Property(i => i.AltText).HasMaxLength(256);
        builder.Property(i => i.SortOrder);
        builder.Property(i => i.IsPrimary);
        
        builder.HasOne(i => i.Listing).WithMany(l => l.Images).HasForeignKey(i => i.ListingId);
    }
}
