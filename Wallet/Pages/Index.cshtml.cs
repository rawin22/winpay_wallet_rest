using GPWebApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using Wallet.Interfaces;
using Wallet.Models;
using Wallet.Pages.Shared.Components;
using Wallet.Services;

namespace Wallet.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly LanguageService _languageService;
        private readonly ITsgCoreServiceHelper _coreHelper;
        public BalanceCardsModel _balanceCardsModel;
        public FxCardsModel _fxCardsModel;
        public PaymentCardsModel _paymentCardsModel;

        public decimal TotalBalance { get; private set; } = 0;
        // Moved initialization to OnGet to avoid initializer errors
        public string CurrentLocale { get; private set; } = "";
        public string FlagCode { get; private set; } = "";
        private String LastName { get; set; } = ""; // Changed to property with setter
        private String Email { get; set; } = ""; // Changed to property with setter
        public String Username { get; set; } = ""; // Changed to property with setter
        public String Greeting { get; set; } = "";
        public string TranslationsJson { get; private set; } = "";
        public Dictionary<string, string> LabelTranslations = new Dictionary<string, string>();
        public string ErrorMessage { get; set; } = "";
        public string Message { get; set; } = "";

        public string CurrentLanguageNameSimplified { get; set; } = "";
        public string CurrentYear { get; set; } = DateTime.Now.Year.ToString();
        //public string WkycVersion { get; set; } = ;
        public bool IsLoading { get; set; } = false;
        public Language[] Languages { get; set; } = Array.Empty<Language>();
        public List<FXDealSearchData> FXDeals { get; private set; } = new();

        public IndexModel(ILogger<IndexModel> logger, LanguageService languageService, IJSRuntime jsRuntime, ITsgCoreServiceHelper coreHelper, IConfiguration configuration, BalanceCardsModel balanceCardsModel, FxCardsModel fxCardsModel, PaymentCardsModel paymentCardsModel
            )
        {
            _logger = logger;
            _coreHelper = coreHelper;
            _configuration = configuration;
            _languageService = languageService;
            _balanceCardsModel = balanceCardsModel;
            _fxCardsModel = fxCardsModel;
            _paymentCardsModel = paymentCardsModel;
        }

        public async Task OnGetAsync()
        {
            // Initialize properties here to avoid field initializer errors
            CurrentLocale = User?.FindFirst("PreferredLanguage")?.Value ?? "en_US";
            LastName = User?.FindFirst("lastName")?.Value ?? "No lastName found";
            Email = User?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "No email found";
            Username = User?.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? "Guest";
            FlagCode = CurrentLocale.Split('_')[1].ToLower();
            Greeting = Translate("Welcome") + " " + Username;

            await LoadLanguagesAsync();
            await LoadTranslationsAsync(CurrentLocale);
            _logger.LogInformation("Hello from Index {username}", Username);
            await _balanceCardsModel.OnGet();
            await _fxCardsModel.OnGet();
            await _paymentCardsModel.LoadDataAsync();
            TotalBalance = _balanceCardsModel.TotalBalance;
            FXDeals = _fxCardsModel.FxDeals;
        }

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
            _logger.LogInformation("We got {LanguagesLength} languages\nCurrent Language is {FullLanguageName}\nFlagCode is {FlagCode}", Languages.Length, fullLanguageName, FlagCode);
        }

        // Define properties and methods here
        public bool TranslationReady()
        {
            return true;
        }

        public string GetMainLanguageName(string fullName)
        {
            var parts = fullName.Split('(');
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
    }
}