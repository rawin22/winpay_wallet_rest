using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using Wallet.Helper; // Required for InstantPaymentHelper
using GPWebApi.DTO; // Required for Instant Payment DTOs
using System.Security.Claims; // Required for ClaimTypes
using System.Globalization; // Required for CultureInfo and DateTimeStyles

namespace Wallet.Pages.History
{
    public class InstantPaymentDetailsModel : PageModel
    {
        private readonly ILogger<InstantPaymentDetailsModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly InstantPaymentHelper _instantPaymentHelper;

        // Property to hold the fetched payment details
        public InstantPaymentGetData? PaymentDetails { get; private set; }
        public string? ErrorMessage { get; private set; }
        public string CurrentUserAlias { get; private set; } = string.Empty; // Needed for Paid/Received logic

        public InstantPaymentDetailsModel(
            ILogger<InstantPaymentDetailsModel> logger,
            IHttpContextAccessor httpContextAccessor,
            InstantPaymentHelper instantPaymentHelper)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _instantPaymentHelper = instantPaymentHelper;
        }

        // OnGetAsync receives the payment ID from the route
        public async Task<IActionResult> OnGetAsync([FromRoute] Guid id)
        {
            var token = GetToken();
            if (token == null) return RedirectToLogin();

            CurrentUserAlias = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;


            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid Payment ID (Guid.Empty) requested.");
                ErrorMessage = "Invalid Payment ID provided.";
                return Page();
            }

            _logger.LogInformation("Fetching details for Payment ID: {PaymentId}", id);

            try
            {
                // Fetch the specific payment details using the helper
                PaymentDetails = await _instantPaymentHelper.GetPaymentAsync(token, id);

                if (PaymentDetails == null)
                {
                    // GetPaymentAsync returns null if not found (404) or on HTTP/JSON errors within the helper
                    ErrorMessage = $"Payment details could not be retrieved for ID {id}. The payment may not exist or there was an error communicating with the service.";
                    _logger.LogWarning("GetPaymentAsync returned null for Payment ID: {PaymentId}. Check previous logs for specific errors (404, HTTP, JSON).", id);
                }
                else
                {
                    _logger.LogInformation("Successfully fetched details for Payment ID: {PaymentId}, Reference: {PaymentReference}", id, PaymentDetails.PaymentReference);
                }
            }
            catch (Exception ex)
            {
                // Catch any unexpected errors during the process
                _logger.LogError(ex, "An unexpected error occurred while fetching details for Payment ID: {PaymentId}", id);
                ErrorMessage = "An unexpected system error occurred while fetching payment details.";
                PaymentDetails = null; // Ensure details are null on error
            }

            return Page();
        }

        // --- Helper methods (similar/copied from PayModel for consistency) ---

        private string? GetToken()
        {
            // Assume "Token" is the claim type storing the bearer token
            var token = _httpContextAccessor.HttpContext?.User?.FindFirst("Token")?.Value;
            if (string.IsNullOrEmpty(token)) { _logger.LogWarning("Auth token not found in user claims ('Token')."); }
            return token;
        }

        private IActionResult RedirectToLogin()
        {
            _logger.LogWarning("Redirecting to login page due to missing token.");
            // Adjust the path to your actual login page
            return RedirectToPage("/Auth/Login");
        }

        public string GetFlagClass(string? currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode)) return "fi-xx"; // 'xx' for unknown
            return currencyCode.Trim().ToLowerInvariant() switch
            {
                // Add your currency to flag mappings
                "usd" => "fi-us",
                "eur" => "fi-eu",
                "thb" => "fi-th",
                "khr" => "fi-kh",
                "gbp" => "fi-gb",
                "jpy" => "fi-jp",
                // Add more as needed...
                _ => "fi-xx" // Default fallback flag
            };
        }

        public string FormatDateTime(string? dateTimeString)
        {
            if (string.IsNullOrWhiteSpace(dateTimeString)) return "N/A";

            // Prioritize DateTimeOffset if your API might return timezone info
            if (DateTimeOffset.TryParse(dateTimeString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var dto))
            {
                // Consider converting to local time if appropriate: dto.ToLocalTime().ToString(...)
                return dto.ToString("dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture); // Example format
            }
            // Fallback to DateTime
            if (DateTime.TryParse(dateTimeString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            {
                return dt.ToString("dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture);
            }
            // Fallback: Try parsing just the date part if time parsing fails
            if (DateTime.TryParse(dateTimeString.Split('T')[0], CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return dt.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
            }


            _logger.LogWarning("Could not parse date/time string using expected formats: {DateTimeString}", dateTimeString);
            return dateTimeString; // Return original string if parsing fails
        }

        // Gets user-friendly text like Paid, Received, Pending, Cancelled
        public string GetUserFriendlyStatus(string? apiStatus, string? fromAlias, string? toAlias)
        {
            if (string.IsNullOrWhiteSpace(apiStatus)) return "Unknown";

            if (apiStatus == "Posted") // Specific handling for Posted based on direction
            {
                bool isIncoming = !string.IsNullOrEmpty(CurrentUserAlias) && toAlias == CurrentUserAlias;
                return isIncoming ? "Received" : "Paid";
            }

            // Handle other statuses
            return apiStatus switch
            {
                "Created" => "Pending",
                "Voided" => "Cancelled",
                _ => apiStatus // Fallback for any other statuses
            };
        }

        // Gets the Bootstrap background class for the status badge
        public string GetStatusBadgeClass(string? apiStatus)
        {
            return apiStatus switch
            {
                "Posted" => "bg-success",           // Green for Paid/Received
                "Created" => "bg-warning text-dark", // Yellow for Pending
                "Voided" => "bg-danger",            // Red for Cancelled
                _ => "bg-secondary"                 // Grey for others
            };
        }
    }
}