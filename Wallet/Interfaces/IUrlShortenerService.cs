using Wallet.Models;

namespace Wallet.Interfaces;

public interface IUrlShortenerService
{
	Task<UrlShortenerResponse> CreateShortUrlAsync(string longUrl);
	Task<UrlShortenerResponse> GetShortUrlDetailAsync(string key);
	string GetKeyFromShortUrl(string shortUrl);
}