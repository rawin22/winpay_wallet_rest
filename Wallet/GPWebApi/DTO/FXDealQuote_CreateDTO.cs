namespace GPWebApi.DTO;

public class FXDealQuoteCreateRequest
{
    public Guid CustomerId { get; set; } 
    public string BuyCurrencyCode { get; set; } = string.Empty;
    public string SellCurrencyCode { get; set; } = string.Empty;
    public decimal Amount { get; set; } = decimal.Zero;
    public string AmountCurrencyCode { get; set; } = string.Empty;
    public string DealType { get; set; } = string.Empty;
    public string WindowOpenDate { get; set; } = string.Empty;
    public string FinalValueDate { get; set; } = string.Empty;
    public bool IsForCurrencyCalculator { get; set; }
}

public class FXDealQuoteCreateResponse : DTOResponseBase
{
    public FXDealQuoteData Quote { get; set; } 
}

public class FXDealQuoteData
{
    public Guid QuoteId { get; set; }
    public string QuoteReference { get; set; }
    public string QuoteSequenceNumber { get; set; }
    public string CustomerAccountNumber { get; set; }
    public string DealType { get; set; }
    public string BuyAmount { get; set; }
    public string BuyCurrencyCode { get; set; }
    public string SellAmount { get; set; }
    public string SellCurrencyCode { get; set; }
    public string Rate { get; set; }
    public string Symbol { get; set; }
    public string DealDate { get; set; }
    public string ValueDate { get; set; }
    public string QuoteTime { get; set; }
    public string ExpirationTime { get; set; }
    public bool IsForCurrencyCalculator { get; set; }
}

