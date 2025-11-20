using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Wallet.Helper; // Required for InstantPaymentHelper
using GPWebApi.DTO; // Required for Instant Payment DTOs
using System.Security.Claims; // Required for ClaimTypes
using System.Globalization; // Required for CultureInfo

namespace Wallet.Pages.Shared.Components
{
    public class PaymentCardsModel : PageModel
    {
        private readonly ILogger<PaymentCardsModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly InstantPaymentHelper _instantPaymentHelper;

        // Public properties to expose data to the Razor view
        public List<InstantPaymentSearchRecord> RecentPayments { get; private set; } = new List<InstantPaymentSearchRecord>();
        public string CurrentUserAlias { get; private set; } = string.Empty;
        public string? ErrorMessage { get; private set; }

        // Constructor for dependency injection
        public PaymentCardsModel(
            ILogger<PaymentCardsModel> logger,
            IHttpContextAccessor httpContextAccessor,
            InstantPaymentHelper instantPaymentHelper)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _instantPaymentHelper = instantPaymentHelper;
        }

        // Renamed from OnGet to avoid conflicts if ever used as a full page.
        // This will be called by the IndexModel.
        public async Task LoadDataAsync()
        {
            _logger.LogInformation("Loading data for PaymentCards component...");

            var token = GetToken();
            if (token == null)
            {
                ErrorMessage = "Authentication token missing. Cannot load payments.";
                // Optionally, you could still return RedirectToPage("/auth/Login") from the parent
                // but setting an error message might be better for a component.
                return;
            }

            CurrentUserAlias = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
            if (string.IsNullOrEmpty(CurrentUserAlias))
            {
                _logger.LogWarning("Could not determine CurrentUserAlias from claims for PaymentCards.");
                // Decide if this is an error or just means we can't determine direction
                // ErrorMessage = "User alias could not be determined.";
                // return;
            }

            try
            {
                // Search for the latest 5 payments
                var searchRequest = new InstantPaymentSearchRequest
                {
                    PageIndex = 0,
                    PageSize = 5, // Limit to 5 for the dashboard view
                    SortBy = InstantPaymentSearchSortBy.CreatedTime,
                    SortDirection = InstantPaymentSortDirection.Descending
                };

                _logger.LogInformation("Fetching recent payments for user alias (context): {UserAlias}", CurrentUserAlias);
                var searchResponse = await _instantPaymentHelper.SearchPaymentsAsync(token, searchRequest);

                if (searchResponse?.Problems != null)
                {
                    ErrorMessage = $"Failed to fetch recent payments. API reported issues: {string.Join("; ", searchResponse.Problems.Select(p => p.Message ?? "Unknown issue"))}";
                    _logger.LogError("API problems fetching recent payments: {Problems}", System.Text.Json.JsonSerializer.Serialize(searchResponse.Problems));
                    RecentPayments = searchResponse.Records?.Payments ?? new List<InstantPaymentSearchRecord>(); // Assign potentially partial data if available
                }
                else if (searchResponse?.Records?.Payments != null)
                {
                    RecentPayments = searchResponse.Records.Payments;
                    _logger.LogInformation("Successfully fetched {Count} recent payment records.", RecentPayments.Count);
                }
                else
                {
                    _logger.LogWarning("SearchPaymentsAsync returned null or empty Records/Payments list without specific API problems.");
                    // Don't set ErrorMessage here, just show "No recent payments" in the view.
                    RecentPayments = new List<InstantPaymentSearchRecord>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching recent payments for the dashboard.");
                ErrorMessage = "An system error occurred fetching recent payments.";
                RecentPayments = new List<InstantPaymentSearchRecord>(); // Ensure empty list on failure
            }
        }

        private string? GetToken()
        {
            // Using the specific claim name "Token" as seen in FxCardsModel
            var token = _httpContextAccessor.HttpContext?.User?.FindFirst("Token")?.Value;
            if (string.IsNullOrEmpty(token)) { _logger.LogWarning("Auth token not found in user claims ('Token')."); }
            return token;
        }

        // Helper to determine if a payment is outgoing based on the current user
        public bool IsOutgoing(InstantPaymentSearchRecord payment)
        {
            return !string.IsNullOrEmpty(CurrentUserAlias) && payment.FromCustomerAlias == CurrentUserAlias;
        }

        // Helper to determine if a payment is incoming
        public bool IsIncoming(InstantPaymentSearchRecord payment)
        {
            return !string.IsNullOrEmpty(CurrentUserAlias) && payment.ToCustomerAlias == CurrentUserAlias;
        }

        // Helper to get the counterparty alias (who the payment was to/from)
        public string GetCounterpartyAlias(InstantPaymentSearchRecord payment)
        {
            if (IsOutgoing(payment)) return payment.ToCustomerAlias ?? "N/A";
            if (IsIncoming(payment)) return payment.FromCustomerAlias ?? "N/A";
            // If direction unknown, show both if different, otherwise one
            if (payment.FromCustomerAlias == payment.ToCustomerAlias) return payment.FromCustomerAlias ?? "N/A";
            return $"{payment.FromCustomerAlias ?? "N/A"} -> {payment.ToCustomerAlias ?? "N/A"}";
        }

        // Helper to format amount string (reusing from History model)
        public string GetFormattedAmount(InstantPaymentSearchRecord payment)
        {
            // You might already have a similar helper or extension method
            return $"{payment.Amount.ToString("N2", CultureInfo.InvariantCulture)} {payment.CurrencyCode}"; // Example formatting
        }

        // Helper to get a simplified status
        public string GetUserFriendlyStatus(string? apiStatus)
        {
            if (string.IsNullOrWhiteSpace(apiStatus)) return "Unknown";
            return apiStatus switch
            {
                "Posted" => "Completed", // Simplified for dashboard
                "Created" => "Pending",
                "Voided" => "Cancelled",
                _ => apiStatus
            };
        }
    }
}