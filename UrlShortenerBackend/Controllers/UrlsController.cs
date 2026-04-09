using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortenerBackend.Data;
using UrlShortenerBackend.DTOs;
using UrlShortenerBackend.Models;

namespace UrlShortenerBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlsController : ControllerBase
    {
        private readonly UrlDbContext _context;
        private const string Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public UrlsController(UrlDbContext context)
        {
            _context = context;
        }

        // 1. API Rút gọn link: POST /api/urls/shorten
        [HttpPost("shorten")]
        public async Task<IActionResult> Shorten([FromBody] UrlDto urlDto)
        {
            // 1. Kiểm tra URL
            if (string.IsNullOrEmpty(urlDto.LongUrl) || !Uri.TryCreate(urlDto.LongUrl, UriKind.Absolute, out _))
            {
                return BadRequest(new { message = "URL không hợp lệ." });
            }

            // 2. Tạo mã ngắn ngẫu nhiên (Thay vì dùng Id tự tăng)
            // Cách này giúp tránh lỗi NULL và tăng tốc độ xử lý
            string newShortCode = Guid.NewGuid().ToString().Substring(0, 6);

            // 3. Khởi tạo đối tượng Url với đầy đủ thông tin (Không để ShortCode bị NULL)
            var newUrl = new Url
            {
                OriginalUrl = urlDto.LongUrl,
                ShortCode = newShortCode, // Gán mã ngay tại đây
                CreateAt = DateTime.UtcNow,
                ClickCount = 0
            };

            // 4. Lưu vào Database (Chỉ cần 1 lần duy nhất)
            _context.Urls.Add(newUrl);
            await _context.SaveChangesAsync();

            // 5. Trả về kết quả
            var shortUrl = $"{Request.Scheme}://{Request.Host}/{newUrl.ShortCode}";
            return Ok(new { shortCode = newUrl.ShortCode, shortUrl = shortUrl });
        }   

        // Hàm hỗ trợ mã hóa Base62
        private string Encode(int id)
        {
            if (id == 0) return Alphabet[0].ToString();
            var s = string.Empty;
            while (id > 0)
            {
                s = Alphabet[id % 62] + s;
                id /= 62;
            }
            return s;
        }
    }
}