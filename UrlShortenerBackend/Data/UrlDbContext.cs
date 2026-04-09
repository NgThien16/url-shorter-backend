using Microsoft.EntityFrameworkCore;

namespace UrlShortenerBackend.Data
{
    public class UrlDbContext:DbContext
    {
        public UrlDbContext(DbContextOptions<UrlDbContext> options) : base(options)
        {
        }
        public DbSet<Models.Url> Urls { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Models.Url>().HasData(
                new Models.Url
                {
                    Id = 1,
                    OriginalUrl = "https://www.google.com",
                    ShortCode = "abc123",
                    CreateAt = new DateTime(2024, 1, 1),
                    ClickCount = 0
                },
                new Models.Url
                {
                    Id = 2,
                    OriginalUrl = "https://www.microsoft.com",
                    ShortCode = "def456",
                    CreateAt = new DateTime(2024, 1, 1),
                    ClickCount = 0
                }
           );
        }
    }
}
