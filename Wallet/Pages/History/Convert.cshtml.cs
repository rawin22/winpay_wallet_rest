using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Wallet.Helper; // Required for FxDealHelper
using GPWebApi.DTO; // Required for FXDealSearchData DTO
using System.Security.Claims;
using System.Globalization;

namespace Wallet.Pages.History
{
    public class ConvertModel : PageModel
    {
        private readonly ILogger<ConvertModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FxDealHelper _fxDealHelper; // Use FxDealHelper

        // Property to hold the fetched deals (using name relevant to page context)
        public List<FXDealSearchData> Conversions { get; private set; } = new List<FXDealSearchData>();
        public string? ErrorMessage { get; private set; }

        public ConvertModel(
            ILogger<ConvertModel> logger,
            IHttpContextAccessor httpContextAccessor,
            FxDealHelper fxDealHelper) // Inject FxDealHelper
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _fxDealHelper = fxDealHelper;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = GetToken();
            if (token == null) return RedirectToLogin();

            try
            {
                _logger.LogInformation("Fetching FX deal history (Convert History).");
                // Use the existing GetFxDealsAsync method
                var dealsList = await _fxDealHelper.GetFxDealsAsync(token);

                if (dealsList != null)
                {
                    Conversions = dealsList;
                    _logger.LogInformation("Successfully fetched {Count} FX deal/conversion history records.", Conversions.Count);

                    // Limitation: GetFxDealsAsync swallows errors and returns empty list.
                    // We can only check if the list is empty, but can't distinguish 'no data' from 'error'.
                    if (!Conversions.Any())
                    {
                        // Set a generic message as we don't know the exact reason for empty list
                        ErrorMessage = "No conversion history found, or unable to retrieve history at this time.";
                        _logger.LogWarning("GetFxDealsAsync returned an empty list. Cause unknown (could be no data or an internal error in the helper).");
                    }
                }
                else
                {
                    // Should ideally not happen based on GetFxDealsAsync implementation, but good practice.
                    ErrorMessage = "Failed to retrieve conversion history.";
                    _logger.LogError("GetFxDealsAsync returned null unexpectedly.");
                    Conversions = new List<FXDealSearchData>();
                }
            }
            catch (Exception ex) // Catch unexpected exceptions during OnGet processing
            {
                _logger.LogError(ex, "An error occurred while processing conversion history request.");
                ErrorMessage = "An unexpected system error occurred.";
                Conversions = new List<FXDealSearchData>(); // Ensure empty list on failure
            }

            return Page();
        }

        private string? GetToken()
        {
            var token = _httpContextAccessor.HttpContext?.User?.FindFirst("Token")?.Value;
            if (string.IsNullOrEmpty(token)) { _logger.LogWarning("Auth token not found in user claims ('Token')."); }
            return token;
        }

        private IActionResult RedirectToLogin()
        {
            _logger.LogWarning("Redirecting to login page due to missing token.");
            return RedirectToPage("/auth/Login"); // Adjust path if needed
        }

        // Helper methods can be reused or adapted from PayModel
        public string GetFlagClass(string? currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode)) return "fi-xx";
            return currencyCode.Trim().ToLowerInvariant() switch
            {
                "usd" => "fi-us",
                "eur" => "fi-eu",
                "thb" => "fi-th",
                "khr" => "fi-kh",
                "gbp" => "fi-gb",
                "jpy" => "fi-jp",
                "aud" => "fi-au",
                "cad" => "fi-ca",
                "chf" => "fi-ch",
                "cny" => "fi-cn",
                "inr" => "fi-in",
                _ => "fi-xx"
            };
        }

        public string FormatDateTime(string? dateTimeString)
        {
            // Reusing the same robust formatting logic from PayModel
            if (string.IsNullOrWhiteSpace(dateTimeString)) return "N/A";
            if (DateTimeOffset.TryParse(dateTimeString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var dto))
            { return dto.ToString("dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture); }
            if (DateTime.TryParse(dateTimeString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            { return dt.ToString("dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture); }
            if (DateTime.TryParse(dateTimeString.Split('T')[0], CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            { return dt.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture); }
            _logger.LogWarning("Could not parse date/time string using expected formats: {DateTimeString}", dateTimeString);
            return dateTimeString;
        }
    }
}