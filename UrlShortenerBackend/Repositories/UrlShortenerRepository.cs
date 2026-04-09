using UrlShortenerBackend.Repositories.Interfaces;
namespace UrlShortenerBackend.Repositories
{
    public class UrlShortenerRepository:IUrlShortenerRepository
    {
        private const string Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public string Encode(int id)
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
