using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Models;
using UrlShortener.Api.Repositories.Interfaces;

namespace UrlShortener.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlsController : ControllerBase
    {
        private readonly IUrlRepository _repository;
        public UrlsController(IUrlRepository repository) { _repository = repository; }

        [HttpGet]
        public async Task<IActionResult> GetUrls() => Ok(await _repository.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> PostUrl([FromBody] string longUrl)
        {
            if (string.IsNullOrWhiteSpace(longUrl))
            {
                return BadRequest(new { message = "URL cannot be blank?" });
            }
            bool isValidUrl = Uri.TryCreate(longUrl, UriKind.Absolute, out Uri uriResult)
                              && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!isValidUrl)
            {
                return BadRequest(new { message = "Invalid URL format. Must start with http or https" });
            }
            var newUrl = new UrlMapping
            {
                OriginalUrl = longUrl,
                ShortCode = Guid.NewGuid().ToString().Substring(0, 6)
            };

            await _repository.AddAsync(newUrl);
            return Ok(newUrl);
        }
        [HttpGet("{shortCode}")]
        public async Task<IActionResult> RedirectToOriginal(string shortCode)
        { 
            var allUrls = await _repository.GetAllAsync();
            var urlMapping = allUrls.FirstOrDefault(u => u.ShortCode == shortCode);

            if (urlMapping == null)
            {
                return NotFound(new { message = "Short code does not exist!" });
            }
            return Redirect(urlMapping.OriginalUrl);
        }
    }
}