using Microsoft.JSInterop;
using System.Text.Json;

namespace Wallet.Services
{
	public class TranslationService
	{
		private readonly IJSRuntime _jsRuntime;
		private readonly LanguageService _languageService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private string _preferredLanguage = "";
		private Dictionary<string, string> _translations = new Dictionary<string, string>();

		public TranslationService(IJSRuntime jsRuntime, IHttpContextAccessor httpContextAccessor, LanguageService languageService)
		{
			_jsRuntime = jsRuntime;
			_httpContextAccessor = httpContextAccessor;
			_languageService = languageService;
		}

		public async Task LoadTranslationsAsync()
		{
			var translationsJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "Translations");
			if (!string.IsNullOrEmpty(translationsJson))
			{
				_translations = JsonSerializer.Deserialize<Dictionary<string, string>>(translationsJson) ?? new Dictionary<string, string>();
			}
			else
			{
				_preferredLanguage = _httpContextAccessor?.HttpContext?.Request.Cookies["UserPreferredLanguage"] ?? "en_US";
				_translations = await _languageService.LoadTranslationsAsync(_preferredLanguage);
			}
		}

		public string Translate(string key)
		{
			if (_translations.TryGetValue(key, out var translation))
			{
				return translation;
			}

			return key; // Return the key itself if no translation is found
		}

		public string getPreferredLanguage()
		{
			return _preferredLanguage;
		}

		public async Task ChangeLanguageAsync(string languageCode)
		{
			_preferredLanguage = languageCode;
			_translations = await _languageService.LoadTranslationsAsync(languageCode);

			// Update the cookie
			/*			if (_httpContextAccessor?.HttpContext?.Response != null)
						{
							_httpContextAccessor.HttpContext.Response.Cookies.Append("UserPreferredLanguage", languageCode, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
						}
			*/
			await _jsRuntime.InvokeVoidAsync("setCookie", "UserPreferredLanguage", languageCode, 365);

			// Signal to reload the page
			await _jsRuntime.InvokeVoidAsync("location.reload");
		}


	}


}
