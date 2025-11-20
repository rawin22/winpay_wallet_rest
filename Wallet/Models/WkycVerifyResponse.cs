using GPWebApi.DTO;

namespace Wallet.Models;

public class WkycVerifyResponse
{
	public bool IsSuccessful { get; set; } = false;
	public List<string> ErrorMessages { get; set; } = new List<string>();
	public List<WkycError> Errors { get; set; } = new List<WkycError>();
	public VerifyData VLink { get; set; } = new VerifyData();
}

public class WkycError
{
	public int Code { get; set; } = 0;
	public string MessageDetail { get; set; }
}