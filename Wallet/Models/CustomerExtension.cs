using System.ComponentModel.DataAnnotations;

namespace Wallet.Models
{
	public enum Gender
	{
		M, // Male
		F, // Female
		O  // Other
	}
	public class CustomerExtension
	{
		// Additional field for linking with WkycCustomerData
		[Required]
		public string WKYCId { get; set; } = string.Empty;
		[Required]
		public string CountryOfIssuance { get; set; } = string.Empty;

		[Required]
		public string IdType { get; set; } = string.Empty;

		[Required]
		public string IdNumber { get; set; } = string.Empty;

		public string IssuerName { get; set; } = string.Empty;

		[Required]
		public DateTime? IssuanceDate { get; set; }

		[Required]
		public DateTime? ExpirationDate { get; set; }

		// Assuming these are file paths
		public string FrontFilePath { get; set; } = string.Empty;
		public string BackFilePath { get; set; } = string.Empty;
		public string SelfieFilePath { get; set; } = string.Empty;

		[Required]
		public Gender Gender { get; set; }
		public string VLinkId { get; set; } = string.Empty;
		public string IsGetVerifiedRequested { get; set; }
		public string GetVerifiedRequestedByUserId { get; set; }
		public string GetVerifiedRequestedByUserName { get; set; }
		public string GetVerifiedRequestedByName { get; set; }
		public string GetVerifiedRequestedDate { get; set; }
	}
}
