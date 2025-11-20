using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Wallet.Interfaces;
using GPWebApi.DTO;
using Wallet.Helper;
using System;

namespace Wallet.Pages.UserProfile
{
    public class PasswordChangeModel : PageModel
    {
        private readonly ILogger<PasswordChangeModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITsgCoreServiceHelper _serviceHelper;
        private readonly PasswordHelper _passwordHelper;

        public PasswordChangeModel(ILogger<PasswordChangeModel> logger, IHttpContextAccessor httpContextAccessor, ITsgCoreServiceHelper serviceHelper, PasswordHelper passwordHelper)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _serviceHelper = serviceHelper;
            _passwordHelper = passwordHelper;
        }

        [BindProperty]
        public PasswordChangeInputModel Input { get; set; } = new PasswordChangeInputModel();

        public string ErrorMessage { get; private set; } = string.Empty;

        // ✅ Add a success message property
        public string SuccessMessage { get; private set; } = string.Empty;


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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var token = _httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;
                if (string.IsNullOrEmpty(token))
                {
                    ErrorMessage = "Session expired. Please log in again.";
                    return Page();
                }

                var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    ErrorMessage = "User ID is invalid.";
                    return Page();
                }

                // Create DTO for API Call
                var request = new UserPasswordChangeRequest
                {
                    UserId = userId,
                    OldPassword = Input.OldPassword,
                    NewPassword = Input.NewPassword
                };

                _logger.LogInformation("User Id {UserId} attempting to change password.", request.UserId);

                var response = await _passwordHelper.ChangePasswordAsync(request);

                // 🔥 FIX: Check if API returned errors and display them
                if (response?.Problems != null && response.Problems.Count > 0)
                {
                    ErrorMessage = string.Join(", ", response.Problems.Select(p => p.MessageDetails));
                    _logger.LogError("Password change failed: {ErrorMessage}", ErrorMessage);
                    return Page(); // Stay on the same page and show the error.
                }

                _logger.LogInformation("User password successfully changed.");
                // ✅ Show success
                SuccessMessage = "Hooray Password changed successfully!";
                // Instead of RedirectToPage("/UserProfile/PasswordChange")
                return Page(); // Stay on the same page so SuccessMessage is displayed
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password");
                ErrorMessage = "An unexpected error occurred while changing the password. Please try again.";
                return Page();
            }
        }


    }

    public class PasswordChangeInputModel
    {
        [Required]
        public string OldPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
