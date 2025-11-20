namespace GPWebApi.DTO;

public class IncomingPaymentDeleteRequest
{
    public Guid IncomingPaymentId { get; set; } 
    public string Timestamp { get; set; } = string.Empty;
}

public class IncomingPaymentDeleteResponse : DTOResponseBase
{
}