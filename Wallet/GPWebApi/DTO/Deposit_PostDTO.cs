namespace GPWebApi.DTO;

public class DepositPostRequest
{
    public Guid DepositId { get; set; }
}

public class DepositPostResponse : DTOResponseBase
{
    public DepositPostData DepositInformation { get; set; } 
}

public class DepositPostData
{
    public Guid DepositId { get; set; }
    public string DepositReference { get; set; } = string.Empty;
    public string Timestamp { get; set; } = string.Empty;
}

