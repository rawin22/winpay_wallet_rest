namespace GPWebApi.DTO;

public class FXDealGetRequest
{
    public Guid FXDealId { get; set; } 
}

public class FXDealGetResponse : DTOResponseBase
{
    public FXDealGetData FXDeal { get; set; }
}

public class FXDealGetData
{
    public Guid FXDealId { get; set; }
    public int FXDealSequenceNumber { get; set; }
    public string FXDealReference { get; set; }
    public Guid FXDealQuoteId { get; set; }
    public string FXDealQuoteReference { get; set; }
    public int FXDealQuoteProviderId { get; set; }
    public bool IsCoveringOtherDeal { get; set; }
    public Guid? CoveringFXDealId { get; set; }
    public string? CoveringFXDealReference { get; set; }
    public Guid? CoveredByFXDealId { get; set; }
    public string? CoveredByFXDealReference { get; set; }
    public string ProviderExecutionID { get; set; }
    public string ProviderTradeID { get; set; }
    public string BranchName { get; set; }
    public string BranchCode { get; set; }
    public Guid BookedByUserId { get; set; }
    public string BookedByUserName { get; set; }
    public Guid BookedForOrganizationId { get; set; }
    public int BookedForOrganizationTypeId { get; set; }
    public Guid BookedForCustomerId { get; set; }
    public string BookedForCustomerName { get; set; }
    public Guid AccountRepresentativeId { get; set; }
    public string AccountRepresentativeName { get; set; }
    public int FXDealTypeId { get; set; }
    public string FXDealTypeName { get; set; }
    public string BookedTime { get; set; }
    public string DealDate { get; set; }
    public Decimal BuyAmount { get; set; }
    public string BuyAmountTextBare { get; set; }
    public string BuyAmountTextWithCurrencyCode { get; set; }
    public string BuyAmountTextWithCommasAndCurrencyCode { get; set; }
    public string BuyCurrencyCode { get; set; }
    public int BuyCurrencyScale { get; set; }
    public Decimal SellAmount { get; set; }
    public string SellCurrencyCode { get; set; }
    public int SellCurrencyScale { get; set; }
    public string SellAmountTextBare { get; set; }
    public string SellAmountTextWithCurrencyCode { get; set; }
    public string SellAmountTextWithCommasAndCurrencyCode { get; set; }
    public string AmountCurrencyCode { get; set; }
    public string DealBaseCurrencyCode { get; set; }
    public string DealCounterCurrencyCode { get; set; }
    public string RateFormat { get; set; }
    public Decimal BookedRate { get; set; }
    public int BookedRateScale { get; set; }
    public string BookedRateTextBare { get; set; }
    public string BookedRateTextWithCurrencyCodes { get; set; }
    public bool IsRateOverridden { get; set; } = false;
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
    public bool IsMarketSpreadInPoints { get; set; } = false;
    public Decimal CurrencyPairSpread { get; set; }
    public Decimal CustomerDealTypeSpread { get; set; }
    public bool IsCustomerDealTypeSpreadInPoints { get; set; } = false;
    public Decimal FinalSpotRate { get; set; }
    public string FinalSpotRateTextBare { get; set; }
    public string FinalSpotRateTextWithCurrencyCodes { get; set; }
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
    public string FeeTemplateFeeAmountTextBare { get; set; }
    public string FeeTemplateFeeAmountTextWithCurrencyCode { get; set; }
    public string FeeTemplateFeeAmountCurrencyCode { get; set; }
    public int FeeTemplateFeeAmountCurrencyScale { get; set; }
    public Decimal PricingTemplateFeeAmount { get; set; }
    public string PricingTemplateFeeAmountCurrencyCode { get; set; }
    public string PricingTemplateFeeAmountTextBare { get; set; }
    public string PricingTemplateFeeAmountTextWithCurrencyCode { get; set; }
    public int PricingTemplateFeeAmountCurrencyScale { get; set; }
    public Decimal CoverRateFeeAmount { get; set; }
    public string CoverRateFeeAmountCurrencyCode { get; set; }
    public int CoverRateFeeAmountCurrencyScale { get; set; }
    public string BankBaseCurrencyCode { get; set; }
    public Decimal DealAmountInBaseCurrency { get; set; }
    public bool IsDeleted { get; set; } = false;
}

