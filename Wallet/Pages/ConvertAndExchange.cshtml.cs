using GPWebApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wallet.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Globalization; // Required for CultureInfo and DateTimeStyles
using System.Security.Claims; // Required for ClaimTypes

namespace Wallet.Pages
{
    public class ConvertAndExchangeModel : PageModel
    {
        private readonly BalanceHelper _balanceHelper;
        private readonly FxDealHelper _fxHelper;
        private readonly IHttpContextAccessor _ctx;
        private readonly ILogger<ConvertAndExchangeModel> _logger;

        public ConvertAndExchangeModel(BalanceHelper balanceHelper,
                                       FxDealHelper fxHelper,
                                       IHttpContextAccessor ctx,
                                       ILogger<ConvertAndExchangeModel> logger)
        {
            _balanceHelper = balanceHelper;
            _fxHelper = fxHelper;
            _ctx = ctx;
            _logger = logger;
        }

        // Properties exposed to Razor view
        public List<CustomerBalanceData> Balances { get; private set; } = [];
        public List<CustomerBalanceData> NonZeroBalances { get; private set; } = [];
        public FXDealQuoteData? Quote { get; private set; }
        [BindProperty]
        public FXDealQuoteCreateRequest? OriginalQuotePayload { get; set; } // Retained for potential future use
        public int SecondsToExpiry { get; private set; }
        [TempData]
        public string? SuccessMessage { get; set; }
        public string ErrorMessage { get; private set; } = string.Empty;

        // GET handler
        public async Task OnGetAsync()
        {
            Quote = null;
            SecondsToExpiry = 0;
            OriginalQuotePayload = null;
            await LoadBalancesAsync();
        }

        // POST handler for Getting Quote - NAME REVERTED
        public async Task<IActionResult> OnPostQuoteAsync(string fromCcy, string toCcy, decimal amount, string amountCcy)
        {
            await LoadBalancesAsync();

            var token = _ctx.HttpContext?.User.FindFirst("Token")?.Value;
            if (string.IsNullOrEmpty(token)) return RedirectToPage("/auth/Login");

            _logger.LogInformation("Requesting FX Quote (Handler: Quote): From {FromCcy}, To {ToCcy}, Amount {Amount} {AmountCcy}", fromCcy, toCcy, amount, amountCcy);

            var (quoteResp, payload) = await _fxHelper.CreateQuoteAsync(token, buyCcy: toCcy, sellCcy: fromCcy, amount: amount, amountCcy: amountCcy);

            OriginalQuotePayload = payload; // Store original payload

            if (quoteResp?.Problems == null && quoteResp?.Quote != null)
            {
                Quote = quoteResp.Quote;
                _logger.LogInformation("Quote received successfully. Quote ID: {QuoteId}, Expires: {ExpiryTime}", Quote.QuoteId, Quote.ExpirationTime);

                if (DateTimeOffset.TryParse(Quote.ExpirationTime, out var expiryTimeOffset))
                {
                    var expiry = expiryTimeOffset.UtcDateTime;
                    SecondsToExpiry = (int)(expiry - DateTime.UtcNow).TotalSeconds - 3;
                    if (SecondsToExpiry < 0) SecondsToExpiry = 0;
                    _logger.LogInformation("Quote expires at {Expiry}. Seconds remaining for UI: {Seconds}", expiry, SecondsToExpiry);
                }
                else if (DateTime.TryParse(Quote.ExpirationTime, out var expiryDateTime))
                {
                    SecondsToExpiry = (int)(expiryDateTime - DateTime.Now).TotalSeconds - 3;
                    if (SecondsToExpiry < 0) SecondsToExpiry = 0;
                    _logger.LogWarning("Parsed ExpirationTime without offset. Expiry: {Expiry}. Seconds remaining for UI: {Seconds}", expiryDateTime, SecondsToExpiry);
                }
                else
                {
                    SecondsToExpiry = 0;
                    _logger.LogError("Could not parse quote ExpirationTime: {ExpiryTimeString}", Quote.ExpirationTime);
                    ErrorMessage = "Quote received, but expiry time is unclear.";
                }
            }
            else
            {
                string problemsText = quoteResp?.Problems != null ? string.Join("; ", quoteResp.Problems.Select(p => p.Message)) : "Unknown quote error.";
                ErrorMessage = $"Unable to obtain quote: {problemsText}";
                _logger.LogError("Failed to obtain quote. Problems: {Problems}", problemsText);
                Quote = null;
                SecondsToExpiry = 0;
            }
            return Page();
        }

        // POST handler for Booking/Transfer - NAME REVERTED
        public async Task<IActionResult> OnPostTransferAsync(Guid quoteId) // Expect QuoteId from form
        {
            var token = _ctx.HttpContext?.User.FindFirst("Token")?.Value;
            if (string.IsNullOrEmpty(token)) return RedirectToPage("/auth/Login");

            if (quoteId == Guid.Empty)
            {
                ErrorMessage = "Invalid Quote ID provided.";
                _logger.LogWarning("OnPostTransferAsync called with empty QuoteId.");
                await LoadBalancesAsync();
                return Page();
            }

            _logger.LogInformation("Attempting to execute transfer (Book & Deposit) for QuoteId: {QuoteId}", quoteId);

            // Calls the combined BookAndInstantDepositAsync helper method
            var response = await _fxHelper.BookAndInstantDepositAsync(token, quoteId);

            if (response?.Problems == null && response?.FXDepositData != null)
            {
                _logger.LogInformation("Transfer successful. Deal Ref: {DealRef}, Deposit Ref: {DepositRef}",
                                       response.FXDepositData.FXDealReference, response.FXDepositData.DepositReference);
                SuccessMessage = $"Conversion successful! Deal Ref: {response.FXDepositData.FXDealReference}, Deposit Ref: {response.FXDepositData.DepositReference}";
                Quote = null; // Clear quote state
                SecondsToExpiry = 0;
                OriginalQuotePayload = null;
                await LoadBalancesAsync();
                return Page(); // Show success message
            }
            else
            {
                string problemsText = response?.Problems != null ? string.Join("; ", response.Problems.Select(p => p.Message)) : "Unknown booking error.";
                ErrorMessage = $"Booking failed: {problemsText}";
                _logger.LogError("Failed to book and deposit QuoteId {QuoteId}. Problems: {Problems}", quoteId, problemsText);
                await LoadBalancesAsync();
                return Page(); // Show error message
            }
        }

        // Helper to load balances
        private async Task LoadBalancesAsync()
        {
            var token = _ctx.HttpContext?.User.FindFirst("Token")?.Value;
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("LoadBalancesAsync: Token is missing, cannot load balances.");
                Balances = []; NonZeroBalances = []; return;
            }
            try
            {
                Balances = await _balanceHelper.GetBalancesAsync(token) ?? [];
                NonZeroBalances = Balances.Where(b => b.BalanceAvailable > 0).OrderBy(b => b.CurrencyCode).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load balances in LoadBalancesAsync.");
                ErrorMessage = (ErrorMessage ?? "") + " Failed to load account balances.";
                Balances = []; NonZeroBalances = [];
            }
        }

        // Helper methods can be reused or adapted from PayModel
        public string GetFlagClass(string? currencyCode)
        { /* ... same as before ... */
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
        { /* ... same as before ... */
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