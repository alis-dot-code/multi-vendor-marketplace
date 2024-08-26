using MarketNest.Domain.Entities;

namespace MarketNest.Infrastructure.Persistence.Seed;

public static class CategorySeed
{
    public static List<Category> GetDefaultCategories() => new()
    {
        new Category { Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Name = "Home Services", Slug = "home-services", Description = "Cleaning, repairs, maintenance", SortOrder = 1, IsActive = true },
        new Category { Id = Guid.Parse("10000000-0000-0000-0000-000000000002"), Name = "Beauty", Slug = "beauty", Description = "Hair, makeup, spa services", SortOrder = 2, IsActive = true },
        new Category { Id = Guid.Parse("10000000-0000-0000-0000-000000000003"), Name = "Tutoring", Slug = "tutoring", Description = "Academic tutoring and lessons", SortOrder = 3, IsActive = true },
        new Category { Id = Guid.Parse("10000000-0000-0000-0000-000000000004"), Name = "Fitness", Slug = "fitness", Description = "Personal training, yoga, coaching", SortOrder = 4, IsActive = true },
        new Category { Id = Guid.Parse("10000000-0000-0000-0000-000000000005"), Name = "Photography", Slug = "photography", Description = "Photo shoots, events, portraits", SortOrder = 5, IsActive = true },
        new Category { Id = Guid.Parse("10000000-0000-0000-0000-000000000006"), Name = "Music", Slug = "music", Description = "Lessons, performances, production", SortOrder = 6, IsActive = true },
        new Category { Id = Guid.Parse("10000000-0000-0000-0000-000000000007"), Name = "Consulting", Slug = "consulting", Description = "Business, career, life coaching", SortOrder = 7, IsActive = true },
        new Category { Id = Guid.Parse("10000000-0000-0000-0000-000000000008"), Name = "Events", Slug = "events", Description = "Planning, catering, venues", SortOrder = 8, IsActive = true }
    };
}
