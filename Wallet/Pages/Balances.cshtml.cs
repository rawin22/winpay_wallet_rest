using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wallet.Helper;
using GPWebApi.DTO;

namespace Wallet.Pages
{
    public class BalancesModel : PageModel
    {
        private readonly ILogger<BalancesModel> _logger;
        private readonly BalanceHelper _balanceHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public List<CustomerBalanceData> Balances { get; private set; } = new();
        public int ViewOption { get; set; } = 2;
        public string ErrorMessage { get; private set; } = string.Empty;

        public BalancesModel(ILogger<BalancesModel> logger, BalanceHelper balanceHelper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _balanceHelper = balanceHelper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> OnGet()
        {
            var token = _httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Access token is missing. Redirecting to login.");
                return RedirectToPage("/auth/Login");
            }

            try
            {
                // ✅ Fetch balances from API
                Balances = await _balanceHelper.GetBalancesAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching balances");
                ErrorMessage = "Failed to load balances.";
            }

            return Page();
        }
    }
}
