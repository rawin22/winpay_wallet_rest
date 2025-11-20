using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering; // Required for SelectList
using System.Security.Claims; // Required for ClaimTypes
using GPWebApi.DTO; // Required for DTOs
using Wallet.Helper; // Required for Helpers
using System; // Required for DateTime
using System.Collections.Generic; // Required for List<>
using System.Linq; // Required for LINQ
using System.Threading.Tasks; // Required for Task

namespace Wallet.Pages
{
    public class PaymentWizardModel : PageModel
    {
        private readonly ILogger<PaymentWizardModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CurrencySettingsHelper _currencySettingsHelper;
        private readonly BalanceHelper _balanceHelper;

        // --- Properties for Data Binding ---
        [BindProperty]
        public string DestinationCountryCode { get; set; }

        [BindProperty]
        public string RemittingCurrencyCode { get; set; }

        [BindProperty]
        public decimal? RemittingAmount { get; set; }

        [BindProperty]
        public string FromAccountCurrencyCode { get; set; }

        [BindProperty]
        public decimal? EquivalentAmount { get; set; }

        [BindProperty]
        public DateTime PaymentDate { get; set; } = DateTime.Today;

        // --- Properties for Display and Dropdowns ---
        public SelectList CountryOptions { get; set; }
        public SelectList RemittingCurrencyOptions { get; set; }
        public SelectList FromAccountCurrencyOptions { get; set; }
        public decimal FromAccountBalance { get; set; } // Display only, calculated from selected FromAccountCurrencyCode
        public string SelectedCountryInfo { get; set; }

        // Store fetched data for reuse within the request
        private List<PaymentCountryData> _countries = new();
        private List<CurrencyData> _availableCurrencies = new();
        private List<CustomerBalanceData> _userBalances = new();

        public PaymentWizardModel(
            ILogger<PaymentWizardModel> logger,
            IHttpContextAccessor httpContextAccessor,
            CurrencySettingsHelper currencySettingsHelper,
            BalanceHelper balanceHelper)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _currencySettingsHelper = currencySettingsHelper;
            _balanceHelper = balanceHelper;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated != true)
            {
                _logger.LogWarning("User is not authenticated. Redirecting to login.");
                return RedirectToPage("/auth/Login");
            }

            var token = user.FindFirst("Token")?.Value;
            var baseCountryCode = user.FindFirst("BaseCountryCode")?.Value;
            var baseCurrencyCode = user.FindFirst("BaseCurrencyCode")?.Value;
            var customerId = user.FindFirst("CustomerId")?.Value;

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(baseCountryCode) || string.IsNullOrEmpty(baseCurrencyCode) || string.IsNullOrEmpty(customerId))
            {
                _logger.LogError("Essential user claims (Token, BaseCountryCode, BaseCurrencyCode, CustomerId) are missing.");
                ModelState.AddModelError(string.Empty, "User session information is incomplete. Please log in again.");
                CountryOptions = new SelectList(new List<PaymentCountryData>());
                RemittingCurrencyOptions = new SelectList(new List<CurrencyData>());
                FromAccountCurrencyOptions = new SelectList(new List<CustomerBalanceData>());
                return Page();
            }

            _logger.LogInformation("Fetching data for Payment Wizard for CustomerId: {CustomerId}", customerId);

            try
            {
                var countriesTask = _currencySettingsHelper.GetPaymentCountriesAsync();
                var currenciesTask = _currencySettingsHelper.GetAvailableCurrenciesAsync();
                var balancesTask = _balanceHelper.GetBalancesAsync(token);

                await Task.WhenAll(countriesTask, currenciesTask, balancesTask);

                _countries = await countriesTask ?? new List<PaymentCountryData>();
                _availableCurrencies = await currenciesTask ?? new List<CurrencyData>();
                _userBalances = await balancesTask ?? new List<CustomerBalanceData>();

                if (!_countries.Any()) _logger.LogWarning("No payment countries were loaded.");
                if (!_availableCurrencies.Any()) _logger.LogWarning("No available currencies were loaded.");
                if (!_userBalances.Any()) _logger.LogWarning("No user balances were loaded.");


                CountryOptions = new SelectList(_countries, nameof(PaymentCountryData.CountryCode), nameof(PaymentCountryData.CountryName));
                RemittingCurrencyOptions = new SelectList(_availableCurrencies, nameof(CurrencyData.CurrencyCode), nameof(CurrencyData.CurrencyCode));
                // Corrected: Use CustomerBalanceData properties as seen in PayNowModel example
                FromAccountCurrencyOptions = new SelectList(
                    _userBalances.Select(b => new SelectListItem
                    {
                        Value = b.CurrencyCode,
                        // Display balance in the dropdown text like PayNowModel
                        Text = $"{b.CurrencyCode} (Available: {b.BalanceAvailable:N2})"
                    }), "Value", "Text"); // Use Value/Text from SelectListItem


                // --- Set Default Values ---
                DestinationCountryCode = _countries.Any(c => c.CountryCode == baseCountryCode) ? baseCountryCode : _countries.FirstOrDefault()?.CountryCode;
                var selectedCountryData = _countries.FirstOrDefault(c => c.CountryCode == DestinationCountryCode);

                RemittingCurrencyCode = selectedCountryData?.DefaultCurrencyCode;
                if (string.IsNullOrEmpty(RemittingCurrencyCode) || !_availableCurrencies.Any(c => c.CurrencyCode == RemittingCurrencyCode))
                {
                    RemittingCurrencyCode = _availableCurrencies.FirstOrDefault()?.CurrencyCode;
                }

                FromAccountCurrencyCode = _userBalances.Any(b => b.CurrencyCode == baseCurrencyCode) ? baseCurrencyCode : _userBalances.FirstOrDefault()?.CurrencyCode;

                // Corrected: Use BalanceAvailable
                var selectedBalance = _userBalances.FirstOrDefault(b => b.CurrencyCode == FromAccountCurrencyCode);
                FromAccountBalance = selectedBalance?.BalanceAvailable ?? 0;

                SelectedCountryInfo = selectedCountryData?.Memo ?? "Country information not available.";

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading data for Payment Wizard.");
                ModelState.AddModelError(string.Empty, "An error occurred while loading payment information. Please try again later.");
                CountryOptions = new SelectList(new List<PaymentCountryData>());
                RemittingCurrencyOptions = new SelectList(new List<CurrencyData>());
                FromAccountCurrencyOptions = new SelectList(new List<CustomerBalanceData>());
            }

            return Page();
        }

        public async Task<IActionResult> OnGetCountryInfoAsync(string destinationCountryCode)
        {
            _logger.LogInformation("Fetching info for country code: {CountryCode}", destinationCountryCode);
            if (string.IsNullOrEmpty(destinationCountryCode))
            {
                return new JsonResult(new { info = "Please select a country." });
            }

            // Re-fetch country list if needed
            if (_countries == null || !_countries.Any())
            {
                _logger.LogWarning("Country list not available in handler, re-fetching.");
                var user = _httpContextAccessor.HttpContext?.User;
                var token = user?.FindFirst("Token")?.Value;
                if (!string.IsNullOrEmpty(token))
                {
                    _countries = await _currencySettingsHelper.GetPaymentCountriesAsync() ?? new List<PaymentCountryData>();
                }
                else
                {
                    _logger.LogError("Cannot re-fetch countries in OnGetCountryInfoAsync: Token is missing.");
                    return new JsonResult(new { info = "Error loading country data (session expired?)." });
                }
            }

            var countryData = _countries.FirstOrDefault(c => c.CountryCode == destinationCountryCode);

            if (countryData == null)
            {
                _logger.LogWarning("Could not find data for country code: {CountryCode}", destinationCountryCode);
                return new JsonResult(new { info = "Information not available for this country." });
            }

            return new JsonResult(new { info = countryData.Memo, defaultCurrency = countryData.DefaultCurrencyCode });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("PaymentWizard POST attempt failed ModelState validation.");
                await PopulateDropdownsOnPostErrorAsync();
                return Page();
            }

            _logger.LogInformation("POST received for Payment Wizard. Destination: {Destination}, Remitting: {Amount} {Currency}, From: {FromAccount}",
                                   DestinationCountryCode, RemittingAmount, RemittingCurrencyCode, FromAccountCurrencyCode);

            try
            {
                // Placeholder for actual payment logic
                _logger.LogInformation("Payment logic placeholder: Redirecting.");
                TempData["SuccessMessage"] = "Payment initiated successfully! (Placeholder)";
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment POST");
                ModelState.AddModelError(string.Empty, "An error occurred while processing your payment. Please try again.");
                await PopulateDropdownsOnPostErrorAsync();
                return Page();
            }
        }

        private async Task PopulateDropdownsOnPostErrorAsync()
        {
            _logger.LogInformation("Repopulating dropdowns after POST error.");
            var token = _httpContextAccessor.HttpContext?.User?.FindFirst("Token")?.Value;
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError("Cannot repopulate dropdowns: Token is missing.");
                CountryOptions = new SelectList(new List<PaymentCountryData>());
                RemittingCurrencyOptions = new SelectList(new List<CurrencyData>());
                FromAccountCurrencyOptions = new SelectList(new List<CustomerBalanceData>());
                return;
            }

            try
            {
                var countriesTask = _currencySettingsHelper.GetPaymentCountriesAsync();
                var currenciesTask = _currencySettingsHelper.GetAvailableCurrenciesAsync();
                var balancesTask = _balanceHelper.GetBalancesAsync(token);

                await Task.WhenAll(countriesTask, currenciesTask, balancesTask);

                _countries = await countriesTask ?? new List<PaymentCountryData>();
                _availableCurrencies = await currenciesTask ?? new List<CurrencyData>();
                _userBalances = await balancesTask ?? new List<CustomerBalanceData>();

                CountryOptions = new SelectList(_countries, nameof(PaymentCountryData.CountryCode), nameof(PaymentCountryData.CountryName), DestinationCountryCode);
                RemittingCurrencyOptions = new SelectList(_availableCurrencies, nameof(CurrencyData.CurrencyCode), nameof(CurrencyData.CurrencyCode), RemittingCurrencyCode);

                // Corrected: Use CustomerBalanceData properties as seen in PayNowModel example
                FromAccountCurrencyOptions = new SelectList(
                    _userBalances.Select(b => new SelectListItem
                    {
                        Value = b.CurrencyCode,
                        Text = $"{b.CurrencyCode} (Available: {b.BalanceAvailable:N2})" // Use BalanceAvailable
                    }), "Value", "Text", FromAccountCurrencyCode); // Preserve selection

                // Recalculate balance and country info
                // Corrected: Use BalanceAvailable
                var selectedBalance = _userBalances.FirstOrDefault(b => b.CurrencyCode == FromAccountCurrencyCode);
                FromAccountBalance = selectedBalance?.BalanceAvailable ?? 0;
                var selectedCountryData = _countries.FirstOrDefault(c => c.CountryCode == DestinationCountryCode);
                SelectedCountryInfo = selectedCountryData?.Memo ?? "Country information not available.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to repopulate dropdowns on POST error.");
                CountryOptions = new SelectList(new List<PaymentCountryData>());
                RemittingCurrencyOptions = new SelectList(new List<CurrencyData>());
                FromAccountCurrencyOptions = new SelectList(new List<CustomerBalanceData>());
                ModelState.AddModelError(string.Empty, "Failed to reload page data after error. Please try again.");
            }
        }
    }
}