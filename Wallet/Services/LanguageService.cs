using Wallet.Models;

namespace Wallet.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
        public Language[]? Languages { get; private set; } = new Language[0];

        public LanguageService(HttpClient http, IConfiguration configuration)
        {
            _http = http;
            _configuration = configuration;
        }

        public async Task LoadLanguagesAsync()
        {
            string baseUrl = _configuration["Win:LingoUrl"] ?? "https://winstantlingo.azurewebsites.net/api"; // Null-coalescing for safety
            Console.WriteLine($"Base LINGO Url: {baseUrl}");
            try
            {
                Languages = await _http.GetFromJsonAsync<Language[]>($"{baseUrl}/languages");
                if (Languages == null)
                {
                    throw new HttpRequestException("No languages returned from Azure Server");
                }
                foreach (var language in Languages)
                {
                    Console.WriteLine($"Language: {language.Locale} - {language.FlagCode}- {language.Name}");
                }
                Console.WriteLine($"Languages loaded. Total: {Languages.Length}");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error fetching languages: {e.Message}");
            }
        }

        public async Task<Dictionary<string, string>> LoadTranslationsAsync(string locale)
        {
            string baseUrl = _configuration["Win:LingoUrl"] ?? "https://winstantlingo.azurewebsites.net/api";
            // Log the baseUrl
            Console.WriteLine("Base URl: ", baseUrl);
            var translations = new Dictionary<string, string>();
            try
            {
                var translationsArray = await _http.GetFromJsonAsync<TranslationItem[]>($"{baseUrl}/download/?platform=WALLET&lang={locale}");
                if (translationsArray == null)
                {
                    throw new HttpRequestException("No translations returned from Azure Server");
                }

                // Use label as key for the dictionary
                foreach (var item in translationsArray)
                {
                    if (!translations.ContainsKey(item.Label))
                    {
                        translations.Add(item.Label, item.Translation);
                    }
                    else
                    {
                        // Log a warning or handle the duplicate label case, if necessary
                        Console.WriteLine($"Duplicate label found: {item.Label}");
                    }
                }
                Console.WriteLine($"====>>>Translations loaded. Total: {translations.Count}");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error fetching translations: {e.Message}");
            }

            return translations;
        }

    }
}
