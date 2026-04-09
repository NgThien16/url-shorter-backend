using System.ComponentModel.DataAnnotations;

namespace UrlShortenerBackend.Models
{
    public class Url
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; } = null!;
        public string? ShortCode { get; set; }

        public DateTime CreateAt { get; set; }
        public int ClickCount { get; set; } = 0;

    }
}
