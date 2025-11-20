using GPWebApi.DTO;

namespace Wallet.Models;

public class WkycItemNoteGetResponse : WkycBaseResponse
{
	public ItemNoteData Note { get; set; }
}
