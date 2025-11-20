namespace GPWebApi.DTO;

public class FXDealQuoteBookRequest
{
    public Guid QuoteId { get; set; }
}

public class FXDealQuoteBookResponse : DTOResponseBase
{
    public FXDealData FXDeal { get; set; } 
}

public class FXDealData
{
    public Guid FXDealId { get; set; } 
    public string FXDealReference { get; set; }
    public string FXDealSequenceNumber { get; set; }
}

