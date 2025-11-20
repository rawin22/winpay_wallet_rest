using GPWebApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Wallet.Helper;                         // <‑‑ add
using System.Security.Claims;

namespace Wallet.Pages.UserProfile
{
    public class LiquidationPreferencesModel : PageModel
    {
        private readonly ILogger<LiquidationPreferencesModel> _logger;
        private readonly IHttpContextAccessor _ctx;
        private readonly LiquidationPreferenceHelper _helper;

        public LiquidationPreferencesModel(
            ILogger<LiquidationPreferencesModel> logger,
            IHttpContextAccessor ctx,
            LiquidationPreferenceHelper helper)          // <‑‑ inject
        {
            _logger = logger;
            _ctx = ctx;
            _helper = helper;
        }

        /* ---------- data exposed to Razor ---------- */
        public List<LiquidationPreferenceData> UsedCurrencies { get; private set; } = [];
        public List<LiquidationPreferenceData> NotUsedCurrencies { get; private set; } = [];
        public string ErrorMessage { get; private set; } = string.Empty;

        /* ---------- GET ---------- */
        public async Task<IActionResult> OnGetAsync()
        {
            var token = _ctx.HttpContext?.User.FindFirst("Token")?.Value;
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Access token missing – redirecting to login.");
                return RedirectToPage("/auth/Login");
            }

            var prefs = await _helper.GetPreferencesAsync(token);

            if (prefs.Count == 0)
            {
                ErrorMessage = "Could not load preferences.";
                return Page();
            }

            UsedCurrencies = prefs.Where(p => p.IsActiveForAutoExchange)
                                     .OrderBy(p => p.LiquidationOrder)
                                     .ToList();

            NotUsedCurrencies = prefs.Where(p => !p.IsActiveForAutoExchange)
                                     .OrderBy(p => p.CurrencyCode)
                                     .ToList();

            return Page();
        }

        /* ---------- POST (Save button) ---------- */
        [BindProperty] public string UsedCurrencyCodes { get; set; } = string.Empty;
        public async Task<IActionResult> OnPostAsync()
        {
            var token = _ctx.HttpContext?.User.FindFirst("Token")?.Value;
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Access token missing – redirecting to login.");
                return RedirectToPage("/auth/Login");
            }

            var list = UsedCurrencyCodes
                       .Split(',', StringSplitOptions.RemoveEmptyEntries)
                       .Select(c => c.Trim().ToUpperInvariant())
                       .ToList();

            var success = await _helper.UpdatePreferencesAsync(token, list);

            if (!success)
                ErrorMessage = "Saving failed – please try again.";

            return await OnGetAsync();                    // reload & show fresh data
        }
    }
}
