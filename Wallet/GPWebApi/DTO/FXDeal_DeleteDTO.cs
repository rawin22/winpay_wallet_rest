namespace GPWebApi.DTO;

public class FXDealDeleteRequest
{
    public Guid FXDealId { get; set; }
    public string Timestamp { get; set; }
}

public class FXDealDeleteResponse : DTOResponseBase
{
}