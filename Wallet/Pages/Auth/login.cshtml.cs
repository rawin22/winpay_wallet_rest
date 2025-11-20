using GPWebApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Wallet.Interfaces;
using Wallet.Models;
using Wallet.Services;
using Microsoft.JSInterop;
using System.Text.RegularExpressions;

namespace Wallet.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly ITsgCoreServiceHelper _tsgCoreServiceHelper;
        private readonly LanguageService _languageService;
        private readonly IConfiguration _configuration;

        public string TranslationsJson { get; private set; } = "";
        public Dictionary<string, string> LabelTranslations = new Dictionary<string, string>();
        [BindProperty] public LoginViewModel Input { get; set; } = new LoginViewModel();

        public string CurrentLocale { get; private set; } = "";
        public string FlagCode { get; private set; } = "";
        private String LastName { get; set; } = ""; // Changed to property with setter
        private String Email { get; set; } = ""; // Changed to property with setter
        public String Username { get; set; } = ""; // Changed to property with setter
        public String Greeting { get; set; } = "";
        public string ErrorMessage { get; set; } = "";
        public string Message { get; set; } = "";

        public string CurrentLanguageNameSimplified { get; set; } = "";
        public string CurrentYear { get; set; } = DateTime.Now.Year.ToString();
        //public string WkycVersion { get; set; } = ;
        public bool IsLoading { get; set; } = false;
        public Language[] Languages { get; set; } = Array.Empty<Language>();



        public LoginModel(ILogger<LoginModel> logger, ITsgCoreServiceHelper tsgCoreServiceHelper, LanguageService languageService, IJSRuntime jsRuntime, ITsgCoreServiceHelper coreHelper, IConfiguration configuration)
        {
            _logger = logger;
            _tsgCoreServiceHelper = tsgCoreServiceHelper;
            _configuration = configuration;
            _languageService = languageService;
        }

        // Handles GET requests
        public async Task OnGetAsync()
        {
            // Initialize properties here to avoid field initializer errors
            //CurrentLocale = User?.FindFirst("PreferredLanguage")?.Value ?? "en_US";
            CurrentLocale = Request.Cookies["UserPreferredLanguage"] ?? User?.FindFirst("PreferredLanguage")?.Value ?? "en_US";

            LastName = User?.FindFirst("lastName")?.Value ?? "No lastName found";
            Email = User?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "No email found";
            Username = User?.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? "Guest";
            FlagCode = CurrentLocale.Split('_')[1].ToLower();
            Greeting = Translate("Welcome") + " " + Username;

            await LoadLanguagesAsync();
            await LoadTranslationsAsync(CurrentLocale);
            _logger.LogInformation("Hello from Login {username}", Username);
        }

        // Handles POST requests for login
        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("Hello: {Username}", Input.Username);

            if (!ModelState.IsValid)
            {
                return Page(); // Return the same page with validation errors
            }

            try
            {
                // Authenticate the user using the service
                AuthenticateResponse response = await _tsgCoreServiceHelper.AuthenticateAsync(Input.Username, Input.Password);

                _logger.LogInformation("Authentication response: {Response}", JsonConvert.SerializeObject(response));

                if (response.Tokens == null)
                {
                    ErrorMessage = "Login failed. Please check your username and password.";
                    return Page(); // Return the same page if login fails
                }



                // Set authentication cookies
                await _tsgCoreServiceHelper.SetAuthenticationCookie(response.Tokens, response.UserSettings);

                _logger.LogInformation("User {Username} successfully logged in.", Input.Username);

                // Redirect to the home page or a specific page
                return LocalRedirect(Url.Content("~/"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                ErrorMessage = "An unexpected error occurred. Please try again.";
                return Page(); // Return the same page in case of an error
            }
        } // end of OnPostAsync
        private async Task LoadTranslationsAsync(string locale)
        {
            // Use LanguageService to load translations
            var translations = await _languageService.LoadTranslationsAsync(locale);
            LabelTranslations = translations;
            // Store translations in LocalStorage
            TranslationsJson = JsonConvert.SerializeObject(translations);
        }
        public string Translate(string label)
        {
            var test = LabelTranslations.ContainsKey(label) ? LabelTranslations[label] : label;
            _logger.LogInformation("Translating label: {Label}", label);
            // Loop through the translations lab;e for debug
            foreach (var item in LabelTranslations)
            {
                _logger.LogInformation("Label: {Label} - Translation: {Translation}", item.Key, item.Value);
            }
            return LabelTranslations.ContainsKey(label) ? LabelTranslations[label] : label;
        }
        private async Task LoadLanguagesAsync()
        {
            if (_languageService.Languages == null || _languageService.Languages.Length == 0)
            {
                await _languageService.LoadTranslationsAsync(CurrentLocale);
            }
            Languages = _languageService.Languages ?? Array.Empty<Language>(); // Null-coalescing assignment

            _logger.LogInformation("We got {LanguagesLength} languages", Languages.Length);
            // Added null check for FirstOrDefault
            var fullLanguageName = Languages.FirstOrDefault(l => l.Locale == CurrentLocale)?.Name ?? "English (default)";
            CurrentLanguageNameSimplified = GetMainLanguageName(fullLanguageName);
            FlagCode = CurrentLocale.Split('_')[1].ToLower();
            _logger.LogInformation("======>> We got {LanguagesLength} languages\nCurrent Language is {FullLanguageName}\nFlagCode is {FlagCode}", Languages.Length, fullLanguageName, FlagCode);
        }

        // Define properties and methods here
        public bool TranslationReady()
        {
            return true;
        }
        public string StripParentheses(string input)
        {
            // Remove anything between ASCII or full-width parentheses:
            // '(' or '（', then any characters except ')' or '）', then ')' or '）'
            var pattern = @"[\(\（][^\)\）]*[\)\）]";
            return Regex.Replace(input, pattern, "").Trim();
        }
        public string GetMainLanguageName(string fullName)
        {
            // Split on both ASCII '(' and full-width '（'
            var parts = fullName.Split(new char[] { '(', '（' }, 2);
            return parts[0].Trim();
        }

        public async Task ChangeLanguage(string locale)
        {
            await _languageService.LoadTranslationsAsync(locale);
            _logger.LogInformation("We changed the language to {Locale}", locale);
        }

        public async Task<IActionResult> OnPostChangeLanguageAsync(string locale)
        {
            // this handles the language change from Browser AJAX as this is  a Razor Page and NOT Blazor
            // TODO: Complain to Microsoft about this
            await ChangeLanguage(locale);
            Response.Cookies.Append("UserPreferredLanguage", locale, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
            return RedirectToPage(); // Redirects to the current page
        }
        public string GetForgotPasswordUrl()
        {
            return _configuration.GetValue<string>("Win:Beta:ForgotPasswordUrl") ?? string.Empty;
        }

    } // end of class
} // end of namespace
