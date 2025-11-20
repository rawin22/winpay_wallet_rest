namespace Wallet.Models
{
	public class WkycUserSettingsData
	{
		// TODO: Add properties for user settings
		// data.UserName = response.UserName;
		public string UserName { get; set; } = "";
		public string EmailAddress { get; set; } = "";
		public string FirstName { get; set; } = "";
		public string LastName { get; set; } = "";
		public string WKYCId { get; set; } = "";
		public string BaseCountryCode { get; set; } = "";
		public string CultureCode { get; set; } = "";
		public string PasswordRegEx { get; set; } = "";
		public string UserId { get; set; } = "";
	}

	public class UserSettingsResponse
	{
		public bool IsSuccessful { get; set; } = false;
		public List<string> ErrorMessages { get; set; } = new List<string>();
		public WkycUserSettingsData Data { get; set; } = new WkycUserSettingsData();

	}
}
