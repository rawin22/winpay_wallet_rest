namespace Wallet.Models;

public class UrlShortenerRequest
{
	public string LongUrl { get; set; } = string.Empty;
}

public class UrlShortenerResponse
{
	public string LongUrl { get; set; } = string.Empty;
	public string ShortUrl { get; set; } = string.Empty;
}
