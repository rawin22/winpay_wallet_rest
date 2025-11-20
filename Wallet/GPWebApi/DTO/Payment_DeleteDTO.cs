
namespace GPWebApi.DTO;

public class PaymentDeleteRequest
{
    public Guid PaymentId { get; set; } 
    public string Timestamp { get; set; } = string.Empty;
}

public class PaymentDeleteResponse : DTOResponseBase
{
}

