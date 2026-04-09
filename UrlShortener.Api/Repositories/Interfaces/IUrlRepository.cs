using UrlShortener.Api.Models;

namespace UrlShortener.Api.Repositories.Interfaces
{
    public interface IUrlRepository
    {
        Task<IEnumerable<UrlMapping>> GetAllAsync();
        Task<UrlMapping> GetByCodeAsync(string code);
        Task AddAsync(UrlMapping url);
    }
}
