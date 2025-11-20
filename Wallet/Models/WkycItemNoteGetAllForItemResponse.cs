using GPWebApi.DTO;

namespace Wallet.Models;

public class WkycItemNoteGetAllForItemResponse : WkycBaseResponse
{
	public List<ItemNoteData> Notes { get; set; }
}
