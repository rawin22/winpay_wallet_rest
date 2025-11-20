namespace GPWebApi.DTO;

public class IncomingPaymentVerifyRequest
{
    public Guid IncomingPaymentId { get; set; } 
    public string Timestamp { get; set; } = string.Empty;
}

public class IncomingPaymentVerifyResponse : DTOResponseBase
{
    public IncomingPaymentVerifyData Payment { get; set; }
}

public class IncomingPaymentVerifyData
{
    public Guid PaymentId { get; set; } 
    public string PaymentReference { get; set; }
    public int WKYCStatusTypeId { get; set; } 
    public string WKYCStatusTypeName { get; set; }
    public string WKYCStatusTypeDescription { get; set; }
    public string Timestamp { get; set; }
}