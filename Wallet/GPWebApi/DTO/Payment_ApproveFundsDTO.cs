namespace GPWebApi.DTO;

public class PaymentApproveFundsRequest
{
    public Guid PaymentId { get; set; }
    public string Timestamp { get; set; } = string.Empty;
}

public class PaymentApproveFundsResponse : DTOResponseBase
{
    public PaymentApproveFundsData Payment { get; set; }
}

public class PaymentApproveFundsData
{
    public Guid PaymentId { get; set; } 
    public string PaymentReference { get; set; }
    public string Timestamp { get; set; } 
}