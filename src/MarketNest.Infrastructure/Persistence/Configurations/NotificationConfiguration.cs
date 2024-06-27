using MarketNest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketNest.Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id).ValueGeneratedOnAdd();
        
        builder.Property(n => n.Type).HasConversion<string>().HasMaxLength(30);
        builder.Property(n => n.Title).HasMaxLength(200).IsRequired();
        builder.Property(n => n.Message).IsRequired().HasMaxLength(1000);
        builder.Property(n => n.Data).HasColumnType("jsonb");
        builder.Property(n => n.IsRead);
        builder.Property(n => n.EmailSent);
        
        builder.HasIndex(n => new { n.UserId, n.IsRead });
        
        builder.HasOne(n => n.User).WithMany(u => u.Notifications).HasForeignKey(n => n.UserId);
    }
}
