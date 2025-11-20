using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Wallet.Pages
{
    public class WithdrawModel : PageModel
    {
        private readonly ILogger<WithdrawModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string ErrorMessage { get; private set; } = string.Empty;

        public WithdrawModel(ILogger<WithdrawModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult OnGet()
        {
            var token = _httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Access token is missing. Redirecting to login.");
                return RedirectToPage("/auth/Login");
            }

            return Page();
        }
    }
}
