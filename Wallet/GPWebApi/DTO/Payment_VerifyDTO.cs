namespace GPWebApi.DTO;

public class PaymentVerifyRequest
{
    public Guid PaymentId { get; set; } 
    public bool AcceptWKYCMatches { get; set; } = false;
    public string Timestamp { get; set; } = string.Empty;
}

public class PaymentVerifyResponse : DTOResponseBase
{
    public PaymentVerifyData Payment {  get; set; }
}

public class PaymentVerifyData
{
    public Guid PaymentId { get; set; } 
    public string PaymentReference { get; set; } = string.Empty;
    public int WKYCStatusTypeId { get; set; } = 0;
    public string WKYCStatusTypeName { get; set; } = String.Empty;
    public string WKYCStatusTypeDescription { get; set; } = String.Empty;
    public string Timestamp { get; set; } = String.Empty;
}