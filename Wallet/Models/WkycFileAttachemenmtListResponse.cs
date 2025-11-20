using GPWebApi.DTO;

namespace Wallet.Models
{
	public class WkycFileAttachemenmtListResponse
	{
		public bool IsSuccessful { get; set; } = false;
		public List<string> ErrorMessages { get; set; } = new List<string>();
		public List<FileAttachmentGetInfo> Data { get; set; } = new List<FileAttachmentGetInfo>();
	}
}
