using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Wallet.Services
{
	public class TSGAuthenticationService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public TSGAuthenticationService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task SignInAsync(string authenticationScheme, ClaimsPrincipal claimsPrincipal, AuthenticationProperties authProperties)
		{
			if (authProperties == null)
			{
				authProperties = new AuthenticationProperties();
			}
			if (_httpContextAccessor.HttpContext == null)
			{
				throw new InvalidOperationException("HttpContext must not be null.");
			}
			await _httpContextAccessor.HttpContext.SignInAsync(authenticationScheme, claimsPrincipal, authProperties);
		}

	}
}
