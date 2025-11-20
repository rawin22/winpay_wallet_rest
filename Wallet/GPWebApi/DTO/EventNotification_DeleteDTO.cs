namespace GPWebApi.DTO;

public class EventNotificationDeleteRequest
{
    public Guid ItemId { get; set; } 
    public int ItemTypeId { get; set; }
    public int EventTypeId { get; set; }
    public string EmailAddress { get; set; } 
}

public class EventNotificationDeleteResponse : DTOResponseBase
{
}