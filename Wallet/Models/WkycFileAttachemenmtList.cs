namespace Wallet.Models
{
	public class WkycFileAttachmentInfo
	{
		public string AttachedBy { get; set; } = string.Empty;
		public string AttachedByName { get; set; } = string.Empty;
		public string AttachedTime { get; set; } = string.Empty; // Consider using DateTime type if appropriate
		public string Attributes { get; set; } = string.Empty; // This might need to be a different type based on the actual content
		public string BankId { get; set; } = string.Empty;
		public bool DeletableByCustomer { get; set; }
		public string DeletedBy { get; set; } = string.Empty;
		public string DeletedByName { get; set; } = string.Empty;
		public string DeletedTime { get; set; } = string.Empty; // Consider using DateTime type if appropriate
		public string FileAttachmentId { get; set; } = string.Empty;
		public int FileAttachmentTypeId { get; set; }
		public string FileDescription { get; set; } = string.Empty;
		public string FileName { get; set; } = string.Empty;
		public long FileSize { get; set; }
		public string GroupName { get; set; } = string.Empty;
		public bool IsDeleted { get; set; }
		public string ParentObjectId { get; set; } = string.Empty;
		public int ParentObjectTypeId { get; set; }
		public bool ViewableByBanker { get; set; }
		public bool ViewableByCustomer { get; set; }

	}
}
