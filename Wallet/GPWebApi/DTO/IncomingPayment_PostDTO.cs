namespace GPWebApi.DTO;

public class IncomingPaymentPostRequest
{
    public Guid IncomingPaymentId { get; set; } 
    public bool AcceptWKYCMatches { get; set; } = false;
    public string Timestamp { get; set; } = string.Empty;
}

public class IncomingPaymentPostResponse : DTOResponseBase
{
    public IncomingPaymentPostData Payment { get; set; }
}

public class IncomingPaymentPostData
{
    public Guid PaymentId { get; set; } 
    public string PaymentReference { get; set; } 
    public string Timestamp { get; set; } 
}