namespace GPWebApi.DTO;
public class ItemAttributeCreateOrUpdateRequest
{
    public Guid ItemId { get; set; } 
    public int ItemTypeId { get; set; }
    public string AttributeKey { get; set; }
    public string AttributeValue { get; set; }
}

public class ItemAttributeCreateOrUpdateResponse : DTOResponseBase
{
}