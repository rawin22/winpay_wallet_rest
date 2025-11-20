//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using Wallet.Interfaces;

//using System.Text.Json;

//using System.Text.Json;
using Wallet.Models;

namespace Wallet.Services;

public class UrlShortenerService : IUrlShortenerService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _configuration;
    public Language[]? Languages { get; private set; } = new Language[0];
    private string _baseUrl;

    public UrlShortenerService(HttpClient http, IConfiguration configuration)
    {
        _http = http;
        _configuration = configuration;
        _baseUrl = _configuration["Win:UrlShortenerUrl"] ?? "https://vo2.tech/api";
        Console.WriteLine($"UrlShortenerService, _baseUrl: {_baseUrl}");
    }

    public async Task<UrlShortenerResponse> CreateShortUrlAsync(string longUrl)
    {
        try
        {
            var data = new UrlShortenerRequest
            {
                LongUrl = longUrl
            };

            //var jsonPayload = JsonSerializer.Serialize(request);
            //Console.WriteLine($"jsonPayload: {JsonSerializer.Serialize(jsonPayload)}");
            // Create HttpContent from JSON payload
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            var response = await _http.PostAsync($"{_baseUrl}/ShortenedUrls", content);
            var rawContent = await response.Content.ReadAsStringAsync();

            // You can log this rawContent to a file or the console for debugging purposes
            Console.WriteLine($"rawContent: {rawContent}");

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    //var options = new JsonSerializerOptions
                    //{
                    //	PropertyNameCaseInsensitive = true, // Adjusts for case sensitivity issues
                    //										// Add more options as needed
                    //};

                    var responseContent = await response.Content.ReadFromJsonAsync<UrlShortenerResponse>();
                    Console.WriteLine($"responseContent: {JsonConvert.SerializeObject(responseContent)}");
                    if (responseContent == null)
                    {
                        return new UrlShortenerResponse()
                        {
                        };
                    }

                    return responseContent;
                }
                catch (JsonException jsonException)
                {
                    // Log the exception details
                    System.Console.WriteLine($"Error creating short URL: {jsonException.Message}");
                    return new UrlShortenerResponse()
                    {
                    };
                }

            }

            return new UrlShortenerResponse()
            {
            };
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error creating short URL: {e.Message}");
            return new UrlShortenerResponse()
            {
            };
        }
    }

    public async Task<UrlShortenerResponse> GetShortUrlDetailAsync(string key)
    {
        try
        {
            //var jsonPayload = JsonSerializer.Serialize(request);
            //Console.WriteLine($"jsonPayload: {JsonSerializer.Serialize(jsonPayload)}");
            // Create HttpContent from JSON payload
            //var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            var response = await _http.GetAsync($"{_baseUrl}/ShortenedUrls/{key}");
            var rawContent = await response.Content.ReadAsStringAsync();

            // You can log this rawContent to a file or the console for debugging purposes
            Console.WriteLine($"rawContent: {rawContent}");

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    //var options = new JsonSerializerOptions
                    //{
                    //	PropertyNameCaseInsensitive = true, // Adjusts for case sensitivity issues
                    //										// Add more options as needed
                    //};

                    var responseContent = await response.Content.ReadFromJsonAsync<UrlShortenerResponse>();
                    Console.WriteLine($"responseContent: {JsonConvert.SerializeObject(responseContent)}");
                    if (responseContent == null)
                    {
                        return new UrlShortenerResponse()
                        {
                        };
                    }

                    return responseContent;
                }
                catch (JsonException jsonException)
                {

                    // Log the exception details
                    System.Console.WriteLine($"Error getting short URL detail: {jsonException.Message}");
                    return new UrlShortenerResponse()
                    {
                    };
                }

            }

            return new UrlShortenerResponse()
            {
            };
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error creating short URL: {e.Message}");
            return new UrlShortenerResponse()
            {
            };
        }
    }

    public string GetKeyFromShortUrl(string shortUrl)
    {
        var key = string.Empty;

        Console.WriteLine($"GetKeyFromShortUrl, shortUrl: {shortUrl}");
        //string url = "https://vo2.tech/KHRFQ1W6WO8";

        // Parse the URL
        Uri uri = new Uri(shortUrl);

        // Get the last segment of the URL path
        string lastSegment = uri.Segments[uri.Segments.Length - 1];
        Console.WriteLine($"GetKeyFromShortUrl, lastSegment: {lastSegment}");

        // Remove any trailing slashes
        if (!string.IsNullOrEmpty(lastSegment))
        {
            key = lastSegment.Trim('/');
        }
        // Output the parameter value
        Console.WriteLine($"GetKeyFromShortUrl, key: {key}");

        return key;
    }
}
