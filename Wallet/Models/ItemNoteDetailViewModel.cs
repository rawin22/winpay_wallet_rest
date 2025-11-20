namespace Wallet.Models;

public class ItemNoteDetailViewModel
{
	public Guid ItemNoteId { get; set; }
	public string NoteText { get; set; }
	public bool ViewableByCustomer { get; set; }
	public string CreatedTime { get; set; }
	public string CreatedBy { get; set; }
	public string CreatedByName { get; set; }
}
