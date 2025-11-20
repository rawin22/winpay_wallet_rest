namespace GPWebApi.DTO;

public class InstantPaymentCreateRequest
{
    public string FromCustomer { get; set; } = string.Empty;
    public string ToCustomer { get; set; } = string.Empty;
    public int PaymentTypeId { get; set; } = 1;
    public decimal Amount { get; set; } = decimal.Zero;
    public string CurrencyCode { get; set; } = string.Empty;
    public string ValueDate { get; set; } = string.Empty;
    public string ReasonForPayment { get; set; } = string.Empty;
    public string ExternalReference { get; set; } = string.Empty;
    public string Memo { get; set; } = string.Empty;
}

public class InstantPaymentCreateResponse : DTOResponseBase
{
    public InstantPaymentCreateData Payment { get; set; }
}


public class InstantPaymentCreateData
{
    public Guid PaymentId { get; set; } 
    public string PaymentReference { get; set; }
    public string Timestamp { get; set; }
}