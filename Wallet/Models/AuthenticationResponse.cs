namespace Wallet.Models
{
	public class AuthenticationResponse
	{
		public bool IsSuccessful { get; set; } = false;
		public string AccessToken { get; set; } = "";
		public List<string> ErrorMessages { get; set; } = new List<string>();
	}
}
