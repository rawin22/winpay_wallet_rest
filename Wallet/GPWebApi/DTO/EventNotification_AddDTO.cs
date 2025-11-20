namespace GPWebApi.DTO;

public class EventNotificationAddRequest
{
    public Guid ItemId { get; set; } 
    public int ItemTypeId { get; set; }
    public int EventTypeId { get; set; }
    public string EmailAddress { get; set; }
}

public class EventNotificationAddResponse : DTOResponseBase
{
}