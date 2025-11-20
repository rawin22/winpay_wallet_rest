namespace Wallet.Models
{
	public class ClientPartnerRequest
	{
		// TODO: Check with Jackie and Stave about this model and the string WKYCID
		public int Id { get; set; } = 0;
		public Partner PartnerId { get; set; } = new Partner();
		public string WKycId { get; set; } = string.Empty; // WKYCId
		public string Status { get; set; } = string.Empty;
		public DateTime? RequestedDate { get; set; } = DateTime.Now;
		public bool? IsAcknowledged { get; set; } = false;
		public DateTime? AcknowledgedDate { get; set; }
		public string AcknowledgedById { get; set; } = string.Empty; // WKYCId
		public bool? IsTransmitted { get; set; } = false;
		public DateTime? TransmittedDate { get; set; }
		public string TransmittedById { get; set; } = string.Empty;  // WKYCId
		public bool? IsOnboarded { get; set; } = false;
		public DateTime? OnboardedDate { get; set; }
		public string OnboardedById { get; set; } = string.Empty; // WKYCId
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public string? CreatedById { get; set; } = string.Empty;
		public DateTime? DeletedAt { get; set; }
		public string? DeletedById { get; set; } = string.Empty;
		public DateTime? ModifiedAt { get; set; }
		public string? ModifiedById { get; set; } = string.Empty;


	}
}
