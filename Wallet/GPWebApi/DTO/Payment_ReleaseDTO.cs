namespace GPWebApi.DTO;

public class PaymentReleaseRequest
{
    public Guid PaymentId { get; set; }
    public string Timestamp { get; set; } = string.Empty;
}

public class PaymentReleaseResponse : DTOResponseBase
{
    public PaymentReleaseData Payment { get; set; }
}

public class PaymentReleaseData
{
    public Guid PaymentId { get; set; } 
    public string PaymentReference { get; set; } 
    public string Timestamp { get; set; } 
}