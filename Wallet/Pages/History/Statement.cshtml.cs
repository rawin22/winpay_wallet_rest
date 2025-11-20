using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GPWebApi.DTO;
using Wallet.Helper;

namespace Wallet.Pages.History
{
    public class StatementModel : PageModel
    {
        private readonly ILogger<StatementModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AccountStatementHelper _accountStatementHelper;

        // Properties to bind the query parameters
        [BindProperty(SupportsGet = true)]
        public Guid AccountId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        // Account statement data from the API
        public CustomerAccountStatementData AccountInfo { get; set; } = new CustomerAccountStatementData();
        public List<CustomerAccountStatementEntryData> Entries { get; set; } = new List<CustomerAccountStatementEntryData>();

        public StatementModel(
            ILogger<StatementModel> logger,
            IHttpContextAccessor httpContextAccessor,
            AccountStatementHelper accountStatementHelper)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _accountStatementHelper = accountStatementHelper;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Authentication check
            var token = _httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Access token is missing. Redirecting to login.");
                return RedirectToPage("/auth/Login");
            }

            if (AccountId == Guid.Empty)
            {
                _logger.LogWarning("AccountId is missing or invalid.");
                TempData["ErrorMessage"] = "Invalid account ID.";
                return Page();
            }

            try
            {
                // Set default date range if not provided (last 90 days)
                var endDate = EndDate ?? DateTime.UtcNow;
                var startDate = StartDate ?? endDate.AddDays(-90);

                // Ensure startDate is not after endDate
                if (startDate > endDate)
                {
                    TempData["ErrorMessage"] = "Start date cannot be after end date.";
                    return Page();
                }

                // Fetch the account statement
                var statementResponse = await _accountStatementHelper.GetAccountStatementAsync(token, AccountId, startDate, endDate);
                AccountInfo = statementResponse.AccountInfo ?? new CustomerAccountStatementData();
                Entries = statementResponse.Entries ?? new List<CustomerAccountStatementEntryData>();

                // Update the bound properties to reflect the actual dates used
                StartDate = startDate;
                EndDate = endDate;

                _logger.LogInformation("Account statement fetched successfully for AccountId: {AccountId}", AccountId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching account statement for AccountId: {AccountId}", AccountId);
                TempData["ErrorMessage"] = "An error occurred while loading the account statement.";
            }

            return Page();
        }
    }
}