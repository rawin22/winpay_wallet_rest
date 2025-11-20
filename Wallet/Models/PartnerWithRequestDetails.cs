namespace Wallet.Models
{
	public class PartnerWithRequestDetails
	{
		// Fields from the Partner table
		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public string? CreatedById { get; set; }
		public DateTime? DeletedAt { get; set; }
		public string? DeletedById { get; set; }
		public DateTime? ModifiedAt { get; set; }
		public string? ModifiedById { get; set; }
		public string? OwnerId { get; set; }
		public string Name { get; set; } = string.Empty;
		public string? GlobalName { get; set; }
		public string? Country { get; set; }
		public string? WkycId { get; set; }
		public string? Website { get; set; }
		public string? Memo { get; set; }
		public string? Location { get; set; }
		public string Logo { get; set; } = string.Empty;

		// Additional fields from ClientPartnerRequest
		public string? RequestStatus { get; set; }
		public DateTime? RequestedDate { get; set; }
		public bool? IsAcknowledged { get; set; }
		public DateTime? AcknowledgedDate { get; set; }
		public string? AcknowledgedById { get; set; }
		public bool? IsTransmitted { get; set; }
		public DateTime? TransmittedDate { get; set; }
		public string? TransmittedById { get; set; }
		public bool? IsOnboarded { get; set; }
		public DateTime? OnboardedDate { get; set; }
		public string? OnboardedById { get; set; }
		public DateTime? RequestCreatedAt { get; set; }
		public string? RequestCreatedById { get; set; }
		public DateTime? RequestDeletedAt { get; set; }
		public string? RequestDeletedById { get; set; }
		public DateTime? RequestModifiedAt { get; set; }
		public string? RequestModifiedById { get; set; }
	}
}
