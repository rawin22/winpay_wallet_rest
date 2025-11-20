using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Wallet.Interfaces;

namespace Wallet.Pages.UserProfile
{
    public class PasswordResetModel : PageModel
    {
        private readonly ILogger<PasswordResetModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITsgCoreServiceHelper _serviceHelper;

        public PasswordResetModel(ILogger<PasswordResetModel> logger, IHttpContextAccessor httpContextAccessor, ITsgCoreServiceHelper serviceHelper)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _serviceHelper = serviceHelper;
        }

        public string ErrorMessage { get; private set; } = string.Empty;

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
        public async Task<IActionResult> OnPostResetPasswordAsync(string userId)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;
                if (string.IsNullOrEmpty(token))
                {
                    ErrorMessage = "Session expired. Please log in again.";
                    return Page();
                }

                // 🔹 Dummy success response (API integration later)
                await Task.Delay(500); // Simulate network delay
                _logger.LogInformation($"Password reset for user {userId} (Dummy Response).");

                return RedirectToPage("/UserProfile/PasswordReset"); // Refresh page after reset
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password");
                ErrorMessage = "An error occurred while resetting the password. Please try again.";
                return Page();
            }
        }
    }
}
