namespace GPWebApi.DTO;

public class PaymentAcceptWKYCMatchesRequest
{
    public Guid PaymentId { get; set; } 
    public string Timestamp { get; set; } = string.Empty;
}

public class PaymentAcceptWKYCMatchesResponse : DTOResponseBase
{
    public PaymentAcceptWKYCMatchesData Payment { get; set; } 
}

public class PaymentAcceptWKYCMatchesData
{
    public Guid PaymentId { get; set; } 
    public string PaymentReference { get; set; } = string.Empty;
    public int WKYCStatusTypeId { get; set; } = 0;
    public string WKYCStatusTypeName { get; set; } = String.Empty;
    public string WKYCStatusTypeDescription { get; set; } = String.Empty;
    public string Timestamp { get; set; } = String.Empty;
}