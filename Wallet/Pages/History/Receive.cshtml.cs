using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Wallet.Pages.History
{
    public class ReceiveModel : PageModel
    {
        private readonly ILogger<ReceiveModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string ErrorMessage { get; private set; } = string.Empty;

        public ReceiveModel(ILogger<ReceiveModel> logger, IHttpContextAccessor httpContextAccessor)
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
