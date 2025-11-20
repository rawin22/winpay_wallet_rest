using GPWebApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wallet.Helper;

namespace Wallet.Pages.Shared.Components
{
    public class FxCardsModel : PageModel
    {
        private readonly ILogger<FxCardsModel> _logger;
        private readonly FxDealHelper _fxDealHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public List<FXDealSearchData> FxDeals { get; private set; } = new();

        public string ErrorMessage { get; private set; } = string.Empty;

        public FxCardsModel(ILogger<FxCardsModel> logger, FxDealHelper fxDealHelper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _fxDealHelper = fxDealHelper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> OnGet()
        {
            _logger.LogWarning("Hello World");
            var token = _httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Access token is missing. Redirecting to login.");
                return RedirectToPage("/auth/login");
            }
            try
            {
                // ✅ Fetch balances from API
                FxDeals = await _fxDealHelper.GetFxDealsAsync(token!);


                _logger.LogInformation("Deals fetched successfully.{fxDeals}", FxDeals);
                // Loop Through the Deals
                foreach (var FxDeal in FxDeals)
                {
                    _logger.LogInformation("Deal fetched successfully.{fxDealAmounbt}", FxDeal.BuyAmount);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching deals");
                ErrorMessage = "Failed to load FX Deals.";
            }
            return Page();
        }
    }
}
