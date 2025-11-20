using GPWebApi.DTO;

namespace Wallet.Models;

public class WkycVerifyMultipleResponse
{
	public bool IsSuccessful { get; set; } = false;
	public List<string> ErrorMessages { get; set; } = new List<string>();
	public List<VerifyData> VLinks { get; set; } = new List<VerifyData>();
}
