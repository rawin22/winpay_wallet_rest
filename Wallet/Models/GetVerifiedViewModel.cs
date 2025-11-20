using System.ComponentModel.DataAnnotations;

namespace Wallet.Models;

public class GetVerifiedViewModel
{
	public Guid FrontFileId { get; set; }
	public string FrontFileData { get; set; } = string.Empty;
	[Required]
	public string CountryOfIssuanceCode { get; set; } = string.Empty;
	public string CountryOfIssuanceName { get; set; } = string.Empty;
	[Required]
	public string IdType { get; set; } = string.Empty;
	[Required]
	public string IdNumber { get; set; } = string.Empty;
	[Required]
	public string LastName { get; set; } = string.Empty;
	[Required]
	public string MiddleName { get; set; } = string.Empty;
	[Required]
	public string FirstName { get; set; } = string.Empty;
	[Required]
	public string Nationality { get; set; } = string.Empty;
	[Required]
	public DateTime? DateOfBirth { get; set; }
	[Required]
	public string CityOfBirth { get; set; } = string.Empty;
	[Required]
	public string Gender { get; set; }
	public string GenderTypeName { get; set; }
	[Required]
	public DateTime? IssuanceDate { get; set; }
	[Required]
	public string IssuerName { get; set; } = string.Empty;
	[Required]
	public DateTime? ExpirationDate { get; set; }
	public DateTime? RequestedDate { get; set; }
	public string RequestedUserId { get; set; }
	public string RequestedUserName { get; set; }
	public string RequestedName { get; set; }
	public string VLinkId { get; set; }
	public string VLinkShortUrl { get; set; }
	public Guid CustomerId { get; set; }
}
