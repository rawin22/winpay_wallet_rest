namespace GPWebApi.DTO;

public class EventNotificationGetAllRequest
{
    public string ItemId { get; set; } = String.Empty;
}

public class EventNotificationGetAllResponse : DTOResponseBase
{
    public List<EventNotificationInfo> Notifications { get; set; } = new List<EventNotificationInfo>();
}

public class EventNotificationInfo
{
    public Guid ItemId { get; set; }
    public int ItemTypeId { get; set; }
    public int EventTypeId { get; set; }
    public string EventName { get; set; }
    public string EventDescription { get; set; }
    public int TargetTypeId { get; set; }
    public string RecipientUserId { get; set; }
    public string RecipientEmail { get; set; }
}