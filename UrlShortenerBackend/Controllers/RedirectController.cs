using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortenerBackend.Data;

namespace UrlShortenerBackend.Controllers
{
    public class RedirectController : Controller
    {
        private readonly UrlDbContext _context;

        public RedirectController(UrlDbContext context) => _context = context;

        [HttpGet("/{code}")]
        public async Task<IActionResult> Index(string code)
        {
            var url = await _context.Urls.FirstOrDefaultAsync(x => x.ShortCode == code);
            if (url == null) return NotFound();

            // Tăng lượt click (Metadata)
            url.ClickCount++;
            await _context.SaveChangesAsync();

            return Redirect(url.OriginalUrl);
        }
    }
}
