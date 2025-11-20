using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Wallet.Helper; // Required for InstantPaymentHelper
using GPWebApi.DTO; // Required for Instant Payment DTOs including Problem, ProblemType
using System.Security.Claims; // Required for ClaimTypes
using System.Globalization; // Required for CultureInfo and DateTimeStyles

namespace Wallet.Pages.History
{
    public class PayModel : PageModel
    {
        private readonly ILogger<PayModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly InstantPaymentHelper _instantPaymentHelper;

        public List<InstantPaymentSearchRecord> Payments { get; private set; } = new List<InstantPaymentSearchRecord>();
        public string CurrentUserAlias { get; private set; } = string.Empty;
        public string? ErrorMessage { get; private set; }

        public PayModel(
            ILogger<PayModel> logger,
            IHttpContextAccessor httpContextAccessor,
            InstantPaymentHelper instantPaymentHelper)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _instantPaymentHelper = instantPaymentHelper;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = GetToken();
            if (token == null) return RedirectToLogin();

            CurrentUserAlias = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
            if (string.IsNullOrEmpty(CurrentUserAlias))
            {
                _logger.LogWarning("Could not determine CurrentUserAlias from claims.");
            }

            try
            {
                var searchRequest = new InstantPaymentSearchRequest
                {
                    PageIndex = 0,
                    PageSize = 25,
                    SortBy = InstantPaymentSearchSortBy.CreatedTime,
                    SortDirection = InstantPaymentSortDirection.Descending
                };

                _logger.LogInformation("Fetching payment history for user alias (context): {UserAlias}", CurrentUserAlias);
                var searchResponse = await _instantPaymentHelper.SearchPaymentsAsync(token, searchRequest);

                // Check for problems using the actual Problem DTO and its Message property
                if (searchResponse?.Problems != null) // Getter returns null if list is empty
                {
                    // *** CORRECTED: Use p.Message ***
                    ErrorMessage = $"Failed to fetch payment history. API reported issues: {string.Join("; ", searchResponse.Problems.Select(p => p.Message ?? "Unknown issue"))}";
                    _logger.LogError("API problems fetching payment history: {Problems}", System.Text.Json.JsonSerializer.Serialize(searchResponse.Problems));

                    if (searchResponse.Records?.Payments != null)
                    {
                        Payments = searchResponse.Records.Payments;
                        _logger.LogWarning("Assigned {Count} payments despite API reporting problems.", Payments.Count);
                    }
                    else
                    {
                        Payments = new List<InstantPaymentSearchRecord>(); // Ensure list is empty
                    }
                }
                else if (searchResponse?.Records?.Payments != null) // No problems reported
                {
                    Payments = searchResponse.Records.Payments;
                    _logger.LogInformation("Successfully fetched {Count} payment history records.", Payments.Count);
                }
                else // No problems, but also no data
                {
                    ErrorMessage = "Could not retrieve payment history data (response or data section was empty).";
                    _logger.LogWarning("SearchPaymentsAsync returned null, or empty Records/Payments list without specific API problems.");
                    Payments = new List<InstantPaymentSearchRecord>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching payment history.");
                ErrorMessage = "An unexpected system error occurred while fetching history.";
                Payments = new List<InstantPaymentSearchRecord>(); // Ensure empty list on failure
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
            if (string.IsNullOrWhiteSpace(dateTimeString)) return "N/A";

            if (DateTimeOffset.TryParse(dateTimeString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var dto))
            { return dto.ToString("dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture); }
            if (DateTime.TryParse(dateTimeString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            { return dt.ToString("dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture); }
            if (DateTime.TryParse(dateTimeString.Split('T')[0], CultureInfo.InvariantCulture, DateTimeStyles.None, out dt)) // Try date part only
            { return dt.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture); }

            _logger.LogWarning("Could not parse date/time string using expected formats: {DateTimeString}", dateTimeString);
            return dateTimeString;
        }

        // Update this method inside the PayModel class in PayModel.cs

        public string GetUserFriendlyStatus(string? apiStatus)
        {
            // Now primarily handles non-"Posted" statuses,
            // as Paid/Received logic will be in the view based on direction.
            if (string.IsNullOrWhiteSpace(apiStatus)) return "Unknown";

            return apiStatus switch
            {
                "Created" => "Pending",
                "Voided" => "Cancelled",   // Or "Voided"
                                           // NOTE: Case for "Posted" is removed, handled in view.
                _ => apiStatus // Fallback for any other unknown statuses
            };
        }

        // This method for badge color remains the same
        public string GetStatusBadgeClass(string? apiStatus)
        {
            return apiStatus switch
            {
                "Posted" => "bg-success",            // Both Paid & Received use green
                "Created" => "bg-warning text-dark", // Pending -> yellow
                "Voided" => "bg-danger",             // Cancelled -> red
                _ => "bg-secondary"
            };
        }
    }
}