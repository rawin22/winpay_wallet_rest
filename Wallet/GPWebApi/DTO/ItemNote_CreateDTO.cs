namespace GPWebApi.DTO;

public class ItemNoteCreateRequest 
{
    public Guid ItemId { get; set; }
    public ItemType? ItemType { get; set; } 
    public string? NoteText { get; set; }
    public string CreatedByAlias {  get; set; } = string.Empty;
    public bool? ViewableByCustomer { get; set; }
}

public class ItemNoteCreateResponse : DTOResponseBase
{
}

