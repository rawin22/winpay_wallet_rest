using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering; // Required for SelectListItem
using System.ComponentModel.DataAnnotations; // Required for validation attributes
using System.Threading.Tasks; // Required for async operations
using System; // Required for DateTime, Guid, Exception
using System.Linq; // Required for Select, Where, Any
using Wallet.Helper; // Required for InstantPaymentHelper and BalanceHelper
using GPWebApi.DTO; // Required for Instant Payment DTOs and CustomerBalanceData
using System.Text.Json; // Required for JsonSerializer
using System.Security.Claims; // Required for ClaimTypes

namespace Wallet.Pages
{
    public class PayNowModel : PageModel
    {
        // Injected Dependencies
        private readonly ILogger<PayNowModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly InstantPaymentHelper _instantPaymentHelper;
        private readonly BalanceHelper _balanceHelper;

        // --- Properties for Form Binding ---
        [BindProperty, Required(ErrorMessage = "Recipient is required."), Display(Name = "To")]
        public string Recipient { get; set; } = string.Empty;

        [BindProperty, Required(ErrorMessage = "Please select a currency."), Display(Name = "Currency")]
        public string SelectedCurrency { get; set; } = string.Empty;

        [BindProperty, Required(ErrorMessage = "Amount is required."), Range(0.01, double.MaxValue, ErrorMessage = "Amount must be positive."), DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [BindProperty, Required]
        public string FromAccount { get; set; } = string.Empty;

        // Optional fields are nullable strings
        [BindProperty]
        public string? Invoice { get; set; } = string.Empty;

        [BindProperty, Display(Name = "Real Reason for Payment")]
        public string? Reason { get; set; } = string.Empty;

        [BindProperty]
        public string? Memo { get; set; } = string.Empty;

        // --- Data for Dropdowns ---
        public List<SelectListItem> AvailableCurrenciesWithBalance { get; set; } = new List<SelectListItem>();

        // --- Status Messages ---
        [TempData]
        public string? SuccessMessage { get; set; }
        [TempData]
        public string? ErrorMessage { get; set; }

        // Constructor
        public PayNowModel(
            ILogger<PayNowModel> logger,
            IHttpContextAccessor httpContextAccessor,
            InstantPaymentHelper instantPaymentHelper,
            BalanceHelper balanceHelper)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _instantPaymentHelper = instantPaymentHelper;
            _balanceHelper = balanceHelper;
        }

        // --- OnGet Handler ---
        public async Task<IActionResult> OnGetAsync()
        {
            var token = GetToken();
            if (token == null) return RedirectToLogin();

            var userIdentifier = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value; // Adjust ClaimTypes.Name if needed
            if (string.IsNullOrEmpty(userIdentifier))
            {
                _logger.LogWarning("Could not find user identifier claim ('{ClaimType}') for logged-in user.", ClaimTypes.Name);
                ErrorMessage = "Your account identifier could not be determined. Cannot initiate payment.";
                FromAccount = string.Empty;
                AvailableCurrenciesWithBalance = new List<SelectListItem>();
                return Page();
            }
            else
            {
                FromAccount = userIdentifier;
                _logger.LogInformation("Setting FromAccount to logged-in user: {UserIdentifier}", userIdentifier);
            }
            await PopulateDropdownsAsync(token);
            return Page();
        }

        // --- POST Handler for "Create" (Creates Payment, Status: Created) ---
        public async Task<IActionResult> OnPostCreateAsync()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(FromAccount)) { ErrorMessage = "Your account identifier is missing..."; await PopulateDropdownsAsync(token); return new PageResult(); }
            if (token == null) return RedirectToLogin();
            if (!ModelState.IsValid) { await PopulateDropdownsAsync(token); return new PageResult(); }

            _logger.LogInformation("OnPostCreateAsync called for user {UserIdentifier}. Attempting to create instant payment.", FromAccount);
            var createRequest = CreateInstantPaymentRequestFromModel();

            try
            {
                var createResponse = await _instantPaymentHelper.CreatePaymentAsync(token, createRequest);
                if (createResponse?.Payment != null && createResponse.Problems == null)
                {
                    _logger.LogInformation("Successfully created instant payment with ID: {PaymentId}, Reference: {Reference}. Status: Created.", createResponse.Payment.PaymentId, createResponse.Payment.PaymentReference);
                    TempData["SuccessMessage"] = $"Payment {createResponse.Payment.PaymentReference} created successfully. It needs to be posted separately.";
                    return RedirectToPage("/Index"); // Redirect after successful creation
                }
                else
                {
                    _logger.LogError("Failed to create instant payment for user {UserIdentifier}. Problems: {Problems}", FromAccount, JsonSerializer.Serialize(createResponse?.Problems));
                    ErrorMessage = "Failed to create the payment. Please check the details and try again.";
                    await PopulateDropdownsAsync(token);
                    return new PageResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during OnPostCreateAsync for user {UserIdentifier}.", FromAccount);
                ErrorMessage = "An unexpected error occurred while creating the payment.";
                await PopulateDropdownsAsync(token);
                return new PageResult();
            }
        }

        // --- POST Handler for "Pay Now" (Creates and Posts Payment Immediately) ---
        public async Task<IActionResult> OnPostPayNowAsync()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(FromAccount)) { ErrorMessage = "Your account identifier is missing..."; await PopulateDropdownsAsync(token); return new PageResult(); }
            if (token == null) return RedirectToLogin();
            if (!ModelState.IsValid) { await PopulateDropdownsAsync(token); return new PageResult(); }

            _logger.LogInformation("OnPostPayNowAsync called for user {UserIdentifier}. Attempting to create and post instant payment.", FromAccount);
            var createRequest = CreateInstantPaymentRequestFromModel();

            try
            {
                // Step 1: Create Payment
                var createResponse = await _instantPaymentHelper.CreatePaymentAsync(token, createRequest);

                // Step 2: Check Create Response and Proceed to Post if Successful
                if (createResponse?.Payment?.PaymentId != null && !string.IsNullOrEmpty(createResponse.Payment.Timestamp) && createResponse.Problems == null)
                {
                    Guid paymentId = createResponse.Payment.PaymentId; // Get ID from create response
                    string timestamp = createResponse.Payment.Timestamp; // Get Timestamp from create response
                    string initialRef = createResponse.Payment.PaymentReference; // Get Reference from create response

                    _logger.LogInformation("Payment {Reference} created successfully (ID: {PaymentId}) for user {UserIdentifier}. Attempting to post immediately.", initialRef, paymentId, FromAccount);

                    var postResponse = await _instantPaymentHelper.PostPaymentAsync(token, paymentId, timestamp);

                    // Step 3: Check Post Response (API call success and no problems reported)
                    if (postResponse != null && postResponse.Problems == null)
                    {
                        // Both Create and Post succeeded according to API status/problems
                        _logger.LogInformation("Successfully posted instant payment ID: {PaymentId} (Ref: {InitialRef}) for user {UserIdentifier}.",
                                               paymentId, initialRef, FromAccount); // Use initialRef and paymentId from create step

                        // *** FIXED: Use initialRef from create step for success message ***
                        SuccessMessage = $"Payment {initialRef} processed successfully.";

                        // Clear form and stay on page
                        ModelState.Clear();
                        Recipient = string.Empty;
                        SelectedCurrency = string.Empty;
                        Amount = 0;
                        Invoice = string.Empty;
                        Reason = string.Empty;
                        Memo = string.Empty;
                        await PopulateDropdownsAsync(token);
                        return new PageResult();
                    }
                    else
                    {
                        // Posting failed after creation succeeded
                        _logger.LogError("Failed to post instant payment ID: {PaymentId} (Ref: {InitialRef}) for user {UserIdentifier} after successful creation. Problems: {Problems}", paymentId, initialRef, FromAccount, JsonSerializer.Serialize(postResponse?.Problems));
                        ErrorMessage = $"Payment {initialRef} was created but could not be posted/confirmed immediately. Please check API response or contact support.";
                        await PopulateDropdownsAsync(token);
                        return new PageResult();
                    }
                }
                else // Creation failed
                {
                    _logger.LogError("Failed to create instant payment during PayNow flow for user {UserIdentifier}. Problems: {Problems}", FromAccount, JsonSerializer.Serialize(createResponse?.Problems));
                    ErrorMessage = "Failed to initiate the payment. Please check the details and try again.";
                    await PopulateDropdownsAsync(token);
                    return new PageResult();
                }
            }
            catch (Exception ex) // Catch unexpected errors during the whole process
            {
                _logger.LogError(ex, "An unexpected error occurred during OnPostPayNowAsync for user {UserIdentifier}.", FromAccount);
                ErrorMessage = "An unexpected error occurred while processing the payment.";
                await PopulateDropdownsAsync(token);
                return new PageResult();
            }
        }

        // --- Helper Methods ---
        private InstantPaymentCreateRequest CreateInstantPaymentRequestFromModel()
        {
            return new InstantPaymentCreateRequest
            {
                FromCustomer = this.FromAccount,
                ToCustomer = this.Recipient,
                PaymentTypeId = 1,
                Amount = this.Amount,
                CurrencyCode = this.SelectedCurrency,
                ValueDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                ReasonForPayment = this.Reason ?? string.Empty,
                ExternalReference = this.Invoice ?? string.Empty,
                Memo = this.Memo ?? string.Empty
            };
        }
        private string? GetToken()
        {
            var token = _httpContextAccessor.HttpContext?.User?.FindFirst("Token")?.Value;
            if (string.IsNullOrEmpty(token)) { _logger.LogWarning("Auth token not found in user claims."); }
            return token;
        }
        private IActionResult RedirectToLogin()
        {
            _logger.LogWarning("Redirecting to login page.");
            return RedirectToPage("/auth/Login");
        }
        private async Task PopulateDropdownsAsync(string? token)
        {
            if (string.IsNullOrEmpty(FromAccount) && token != null)
            {
                FromAccount = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
                if (string.IsNullOrEmpty(FromAccount)) { _logger.LogWarning("Could not determine FromAccount during PopulateDropdownsAsync."); }
            }
            if (token == null)
            {
                AvailableCurrenciesWithBalance = new List<SelectListItem>();
                _logger.LogWarning("PopulateDropdownsAsync called with null token. Cannot fetch balances.");
                return;
            }
            try
            {
                _logger.LogInformation("Repopulating balances dropdown using BalanceHelper.");
                var balances = await _balanceHelper.GetBalancesAsync(token);
                if (balances == null)
                {
                    _logger.LogWarning("BalanceHelper returned null list during dropdown repopulation.");
                    balances = new List<CustomerBalanceData>();
                }
                AvailableCurrenciesWithBalance = balances
                    .Where(b => b.BalanceAvailable > 0)
                    .Select(b => new SelectListItem { Value = b.CurrencyCode, Text = $"{b.CurrencyCode} (Available: {b.BalanceAvailable:N2})" }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to re-populate balances dropdown during POST error handling using BalanceHelper.");
                AvailableCurrenciesWithBalance = new List<SelectListItem>();
                ErrorMessage = (ErrorMessage ?? "") + " Failed to reload balance information.";
            }
        }
    }
}