using GPWebApi.DTO;

namespace Wallet.Models;

public class ItemNoteCreateViewModel
{
	public Guid ItemId { get; set; }
	public ItemType? ItemType { get; set; }
	public string? NoteText { get; set; }
	public bool? ViewableByCustomer { get; set; }
}
