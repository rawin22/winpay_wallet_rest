namespace GPWebApi.DTO;

public class FXDealQuoteBookAndInstantDepositRequest
{
    public Guid QuoteId { get; set; }
}

public class FXDealQuoteBookAndInstantDepositResponse : DTOResponseBase
{
    public FXDepositData FXDepositData { get; set; } 
}

public class FXDepositData
{
    public Guid FXDealId { get; set; } 
    public string FXDealReference { get; set; }
    public Guid DepositId { get; set; }
    public string DepositReference { get; set; }
}

