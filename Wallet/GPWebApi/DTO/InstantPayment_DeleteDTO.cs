
namespace GPWebApi.DTO;

public class InstantPaymentDeleteRequest
{
    public Guid InstantPaymentId { get; set; }
    public string Timestamp { get; set; } = string.Empty;
}

public class InstantPaymentDeleteResponse : DTOResponseBase
{
}

