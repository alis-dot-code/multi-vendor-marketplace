using MarketNest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketNest.Infrastructure.Persistence.Configurations;

public class ReviewReplyConfiguration : IEntityTypeConfiguration<ReviewReply>
{
    public void Configure(EntityTypeBuilder<ReviewReply> builder)
    {
        builder.ToTable("review_replies");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedOnAdd();
        
        builder.HasIndex(r => r.ReviewId).IsUnique();
        
        builder.Property(r => r.Comment).IsRequired().HasMaxLength(1000);
        
        builder.HasOne(r => r.Review).WithOne(re => re.Reply).HasForeignKey<ReviewReply>(r => r.ReviewId);
        builder.HasOne(r => r.Vendor).WithMany(v => v.ReviewReplies).HasForeignKey(r => r.VendorId);
    }
}
