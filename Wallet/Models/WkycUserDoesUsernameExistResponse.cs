namespace Wallet.Models
{
	public class WkycUserDoesUsernameExistResponse : WkycBaseResponse
	{
		public bool Exists { get; set; } = false;
	}
}
