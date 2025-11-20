namespace GPWebApi.DTO;

public class FXDealQuoteGetRequest
{
    public Guid FXDealQuoteId { get; set; } 
}

public class FXDealQuoteGetResponse : DTOResponseBase
{
    public FXDealQuoteGetData Quote { get; set; }
}

public class FXDealQuoteGetData
{
    public Guid FXDealQuoteId { get; set; }
    public int FXDealQuoteSequenceNumber { get; set; } 
    public string FXDealQuoteReference { get; set; }
    public Guid? BookedFXDealId { get; set; }
    public string? BookedFXDealReference { get; set; }
    public int FXDealQuoteProviderId { get; set; } 
    public Guid? CoversFXDealQuoteId { get; set; }
    public int RateFeedProviderId { get; set; }
    public string ProviderQuoteId { get; set; }
    public string BranchName { get; set; }
    public string BranchCode { get; set; }
    public Guid QuotedByUserId { get; set; }
    public string QuotedByUserName { get; set; }
    public Guid QuotedForCustomerId { get; set; }
    public string QuotedForCustomerName { get; set; }
    public Guid QuotedForOrganizationId { get; set; }
    public int QuotedForOrganizationTypeId { get; set; }
    public Guid AccountRepresentativeId { get; set; }
    public string AccountRepresentativeName { get; set; }
    public string FXDealQuoteType { get; set; }
    public string FXDealQuoteTypeName { get; set; }
    public int QuoteRequestStatusTypeId { get; set; }
    public string QuoteRequestStatusTypeName { get; set; }
    public string QuoteRequestRejectReasonText { get; set; }
    public bool IsFilled { get; set; }
    public bool IsBooked { get; set; }
    public string QuoteTime { get; set; }
    public string ExpirationTime { get; set; }
    public string DealDate { get; set; }
    public int FXDealTypeId { get; set; }
    public string FXDealTypeName { get; set; }
    public Decimal BuyAmount { get; set; }
    public string BuyAmountTextBare { get; set; }
    public string BuyAmountTextWithCurrencyCode { get; set; }
    public string BuyCurrencyCode { get; set; }
    public int BuyCurrencyScale { get; set; }
    public Decimal SellAmount { get; set; }
    public string SellCurrencyCode { get; set; }
    public int SellCurrencyScale { get; set; }
    public string SellAmountTextBare { get; set; }
    public string SellAmountTextWithCurrencyCode { get; set; }
    public string AmountCurrencyCode { get; set; }
    public string QuoteBaseCurrencyCode { get; set; }
    public string QuoteCounterCurrencyCode { get; set; }
    public string RateFormat { get; set; }
    public Decimal QuotedRate { get; set; }
    public int QuotedRateScale { get; set; }
    public string QuotedRateTextBare { get; set; }
    public string QuotedRateTextWithCurrencyCodes { get; set; }
    public bool IsRateOverridden { get; set; }
    public Decimal OverrideRate { get; set; }
    public Decimal OverrideSpread { get; set; }
    public Decimal Bid { get; set; }
    public Decimal Ask { get; set; }
    public Decimal BuyCurrencyBid { get; set; }
    public Decimal BuyCurrencyAsk { get; set; }
    public Decimal SellCurrencyBid { get; set; }
    public Decimal SellCurrencyAsk { get; set; }
    public Decimal MarketSpotRate { get; set; }
    public string MarketSpotRateTextBare { get; set; }
    public string MarketSpotRateTextWithCurrencyCodes { get; set; }
    public Decimal MarketForwardRate { get; set; }
    public string MarketForwardRateTextBare { get; set; }
    public string MarketForwardRateTextWithCurrencyCodes { get; set; }
    public Decimal MarketSpread { get; set; }
    public bool IsMarketSpreadInPoints { get; set; }
    public Decimal CurrencyPairSpread { get; set; }
    public Decimal CustomerDealTypeSpread { get; set; }
    public bool IsCustomerDealTypeSpreadInPoints { get; set; }
    public Decimal FinalSpotRate { get; set; }
    public Decimal ForwardPoints { get; set; }
    public Decimal FinalForwardRate { get; set; }
    public string FinalForwardRateTextBare { get; set; }
    public string FinalForwardRateTextWithCurrencyCodes { get; set; }
    public Decimal CoverRate { get; set; }
    public string CoverRateTextBare { get; set; }
    public string CoverRateTextWithCurrencyCodes { get; set; }
    public string SpotValueDate { get; set; }
    public string WindowOpenDate { get; set; }
    public string FinalValueDate { get; set; }
    public int NumberOfForwardDays { get; set; }
    public Decimal FeeTemplateFeeAmount { get; set; }
    public string FeeTemplateFeeAmountCurrencyCode { get; set; }
    public int FeeTemplateFeeAmountCurrencyScale { get; set; }
    public Decimal PricingTemplateFeeAmount { get; set; }
    public string PricingTemplateFeeAmountCurrencyCode { get; set; }
    public int PricingTemplateFeeAmountCurrencyScale { get; set; }
    public Decimal CoverRateFeeAmount { get; set; }
    public string CoverRateFeeAmountCurrencyCode { get; set; }
    public int CoverRateFeeAmountCurrencyScale { get; set; }
    public string BankBaseCurrencyCode { get; set; }
    public Decimal DealAmountInBaseCurrency { get; set; }
    public bool IsVisible { get; set; }
}

