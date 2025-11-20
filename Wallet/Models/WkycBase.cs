namespace Wallet.Models
{
	public class WkycBaseResponse
	{
		public bool IsSuccessful { get; set; } = false;
		public List<string> ErrorMessages { get; set; } = new List<string>();
	}
}
