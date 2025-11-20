
namespace GPWebApi.DTO;

public class InstantPaymentPostRequest
{
    public Guid InstantPaymentId { get; set; }
    public string Timestamp { get; set; } = string.Empty;
}

public class InstantPaymentPostResponse : DTOResponseBase
{
    public InstantPaymentPostData Payment { get; set; }
}

public class InstantPaymentPostData
{
    public Guid PaymentId { get; set; } 
    public string PaymentReference { get; set; } = string.Empty;
}