
namespace GPWebApi.DTO;

public class VerifiedLinkDeleteRequest
{
    public Guid VerifiedLinkId { get; set; } 
    public string Timestamp { get; set; } = string.Empty;
}

public class VerifiedLinkDeleteResponse : DTOResponseBase
{
}

