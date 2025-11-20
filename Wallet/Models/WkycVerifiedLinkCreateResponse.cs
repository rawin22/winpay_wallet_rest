using GPWebApi.DTO;

namespace Wallet.Models;

public class WkycVerifiedLinkCreateResponse
{
	public bool IsSuccessful { get; set; } = false;
	public List<string> ErrorMessages { get; set; } = new List<string>();
	public VerifiedLinkCreateData VerifiedLink { get; set; } = new VerifiedLinkCreateData();
}
