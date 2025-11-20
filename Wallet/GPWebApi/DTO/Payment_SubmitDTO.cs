namespace GPWebApi.DTO;

public class PaymentSubmitRequest
{
    public Guid PaymentId { get; set; }
    public string Timestamp { get; set; }
}

public class PaymentSubmitResponse : DTOResponseBase
{
    public PaymentSubmitData Payment { get; set; }
}

public class PaymentSubmitData
{
    public Guid PaymentId { get; set; }
    public string PaymentReference { get; set; } 
    public string Timestamp { get; set; } 
}