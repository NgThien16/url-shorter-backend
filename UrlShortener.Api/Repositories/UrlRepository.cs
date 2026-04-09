using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Data;
using UrlShortener.Api.Repositories.Interfaces;
using UrlShortener.Api.Models;

namespace UrlShortener.Api.Repositories
{
    public class UrlRepository : IUrlRepository
    {
        private readonly UrlDbContext _context;
        public UrlRepository(UrlDbContext context) { _context = context; }

        public async Task<IEnumerable<UrlMapping>> GetAllAsync() => await _context.Urls.ToListAsync();
        public async Task<UrlMapping> GetByCodeAsync(string code) => await _context.Urls.FirstOrDefaultAsync(u => u.ShortCode == code);
        public async Task AddAsync(UrlMapping url)
        {
            _context.Urls.Add(url);
            await _context.SaveChangesAsync();
        }
    }
}