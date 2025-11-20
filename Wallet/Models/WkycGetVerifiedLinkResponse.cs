using GPWebApi.DTO;

namespace Wallet.Models;

public class WkycGetVerifiedLinkResponse
{
	public bool IsSuccessful { get; set; } = false;
	public List<string> ErrorMessages { get; set; } = new List<string>();
	public VerifiedLinkData Data { get; set; } = new VerifiedLinkData();
}
