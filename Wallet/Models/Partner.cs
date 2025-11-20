namespace Wallet.Models
{
	public class Partner
	{
		public int Id { get; set; } = 0;
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public string? CreatedById { get; set; } = string.Empty;
		public DateTime? DeletedAt { get; set; }
		public string? DeletedById { get; set; } = string.Empty;
		public DateTime? ModifiedAt { get; set; }
		public string? ModifiedById { get; set; } = string.Empty;
		public string? OwnerId { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string? GlobalName { get; set; } = string.Empty;
		public string? Country { get; set; } = string.Empty;
		public string? WkycId { get; set; } = string.Empty;
		public string? Website { get; set; } = string.Empty;
		public string? Memo { get; set; } = string.Empty;
		public string? Location { get; set; } = string.Empty;
		public string Logo { get; set; } = string.Empty;
	}
}
