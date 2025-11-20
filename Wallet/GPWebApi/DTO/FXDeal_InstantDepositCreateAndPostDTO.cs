namespace GPWebApi.DTO;

public class FXDealInstantDepositCreateAndPostRequest
{
    public string FXDealId { get; set; } = string.Empty;
}

public class FXDealInstantDepositCreateAndPostResponse : DTOResponseBase
{
    public FXDepositData FXDepositData { get; set; }
}

