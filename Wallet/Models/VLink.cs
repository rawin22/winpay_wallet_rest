using GPWebApi.DTO;

namespace Wallet.Models;


public class VLinkViewModel : VerifiedLinkData
{
	public bool ShareName { get; set; }
	public bool ShareEnglishName { get; set; }
	public string IdType { get; set; }
	public string IdNumber { get; set; }
	public string IssuerName { get; set; }
	public string IssuanceDate { get; set; }
	public string IdExpirationDate { get; set; }
	public string IdFrontFile { get; set; }
	public string IdBackFile { get; set; }
	public string SelfieFile { get; set; }
	public string DocumentDescription { get; set; }
}

public class ZKQRViewModel : VerifiedLinkData
{
	public bool ShareName { get; set; }
	public bool ShareEnglishName { get; set; }
}

public class VLinkDetailModel
{
	public string StealthId { get; set; }
	public string AccessAvailable { get; set; }
}
