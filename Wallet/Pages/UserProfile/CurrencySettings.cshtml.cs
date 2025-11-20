using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wallet.Helper;
using GPWebApi.DTO;

namespace Wallet.Pages.UserProfile
{
    public class CurrencySettingsModel : PageModel
    {
        private readonly ILogger<CurrencySettingsModel> _logger;
        private readonly CurrencySettingsHelper _currencyHelper;

        public List<CurrencyData> Currencies { get; private set; } = new();
        public List<string> FavoriteCurrencies { get; private set; } = new();
        public string ErrorMessage { get; private set; } = "";

        public CurrencySettingsModel(ILogger<CurrencySettingsModel> logger, CurrencySettingsHelper currencyHelper)
        {
            _logger = logger;
            _currencyHelper = currencyHelper;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = User.FindFirst("Token")?.Value;
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Access token is missing. Redirecting to login.");
                return RedirectToPage("/auth/Login");
            }

            try
            {
                Currencies = await _currencyHelper.GetAvailableCurrenciesAsync();
                FavoriteCurrencies = await _currencyHelper.GetUserFavoriteCurrenciesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error loading currency settings: {Message}", ex.Message);
                ErrorMessage = "An error occurred while loading currencies.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostToggleFavoriteAsync(string currencyCode)
        {
            bool success = await _currencyHelper.ToggleFavoriteCurrencyAsync(currencyCode);
            if (!success)
            {
                return BadRequest(new { success = false, message = "Update failed." });
            }

            return new JsonResult(new { success = true, currencyCode });
        }
    }
}
