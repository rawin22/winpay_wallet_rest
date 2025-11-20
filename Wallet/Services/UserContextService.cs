using Microsoft.AspNetCore.Components.Authorization;
namespace Wallet.Services
{

	public class UserContextService
	{
		private readonly AuthenticationStateProvider _authenticationStateProvider;

		public UserContextService(AuthenticationStateProvider authenticationStateProvider)
		{
			_authenticationStateProvider = authenticationStateProvider;
		}

		public async Task<string> GetCurrentUserWkycIdAsync()
		{
			var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
			var user = authState.User;

			if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
			{
				return user.FindFirst("WKYCId")?.Value ?? string.Empty;
			}

			return string.Empty;
		}

		// You can add more methods here to get other user-specific details as needed
	}
}
