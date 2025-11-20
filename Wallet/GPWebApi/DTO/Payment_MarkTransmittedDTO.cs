namespace GPWebApi.DTO;

public class PaymentMarkTransmittedRequest
{
    public Guid PaymentId { get; set; } 
    public string Timestamp { get; set; } = string.Empty;
}

public class PaymentMarkTransmittedResponse : DTOResponseBase
{
}