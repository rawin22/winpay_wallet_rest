using GPWebApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wallet.Helper;

namespace Wallet.Pages.Shared.Components
{
    public class BalanceCardsModel : PageModel
    {
        private readonly ILogger<BalanceCardsModel> _logger;
        private readonly BalanceHelper _balanceHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public List<CustomerBalanceData> Balances { get; private set; } = new();

        public decimal TotalBalance { get; private set; } = 0;
        public int ViewOption { get; set; } = 2;
        public string ErrorMessage { get; private set; } = string.Empty;
        [BindProperty(SupportsGet = true)] public bool FavoritesOnly { get; set; } = false;
        [BindProperty(SupportsGet = true)] public bool HideZero { get; set; } = true;


        public BalanceCardsModel(ILogger<BalanceCardsModel> logger, BalanceHelper balanceHelper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _balanceHelper = balanceHelper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> OnGet()
        {
            _logger.LogWarning("Hello Word");
            var token = _httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Access token is missing. Redirecting to login.");
                return RedirectToPage("/auth/login");
            }
            try
            {
                // ✅ Fetch balances from API
                Balances = await _balanceHelper.GetBalancesAsync(token!);

                TotalBalance = Balances.Sum(b => b.BalanceAvailableBase);


                _logger.LogInformation("Balances fetched successfully.{Balances}", Balances);
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
