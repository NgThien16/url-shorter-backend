using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Models;

namespace UrlShortener.Api.Data
{
    public class UrlDbContext : DbContext
    {
        public UrlDbContext(DbContextOptions<UrlDbContext> options) : base(options) { }

        public DbSet<UrlMapping> Urls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Dữ liệu mẫu (Seed data)
            modelBuilder.Entity<UrlMapping>().HasData(
                new UrlMapping { Id = 1, OriginalUrl = "https://google.com", ShortCode = "goog", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );
        }
    }
}
