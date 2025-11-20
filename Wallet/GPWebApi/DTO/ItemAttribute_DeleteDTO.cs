namespace GPWebApi.DTO;

public class ItemAttributeDeleteRequest
{
    public Guid ItemId { get; set; }
    public string AttributeKey { get; set; }
}

public class ItemAttributeDeleteResponse : DTOResponseBase
{
}