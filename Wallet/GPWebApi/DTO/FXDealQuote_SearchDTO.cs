using System.ComponentModel;
using System.Runtime.Serialization;

namespace GPWebApi.DTO;

public class FXDealQuoteSearchRequest
{
    [DefaultValue("0")]
    public int PageIndex { get; set; } = 0;
    [DefaultValue("25")]
    public int PageSize { get; set; } = 0;
    public FXDealQuoteSearchSortBy SortBy { get; set; } = FXDealQuoteSearchSortBy.None;
    public FXDealQuoteSearchSortDirection SortDirection { get; set; } = FXDealQuoteSearchSortDirection.Descending;
    public FXDealQuoteSearchOptions SearchOptions { get; set; } = new FXDealQuoteSearchOptions();   
}

public enum FXDealQuoteSearchSortBy
{
    [EnumMember]
    None = 0,
    [EnumMember]
    QuoteReference = 1,
    [EnumMember]
    CustomerName = 2,
    [EnumMember]
    BranchName = 3,
    [EnumMember]
    BranchCode = 4,
    [EnumMember]
    BuyAmount = 5,
    [EnumMember]
    BuyCurrencyCode = 6,
    [EnumMember]
    SellAmount = 7,
    [EnumMember]
    SellCurrencyCode = 8,
    [EnumMember]
    QuotedRate = 9,
    [EnumMember]
    MarketRate = 10,
    [EnumMember]
    ValueDate = 11,
    [EnumMember] 
    IsBooked = 12,
    [EnumMember] 
    QuoteTime = 13
}

public enum FXDealQuoteSearchSortDirection
{
    [EnumMember]
    Ascending = 0,
    [EnumMember]
    Descending = 1
}

public class FXDealQuoteSearchOptions
{
    public string FXDealQuoteReference { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public FXDealQuoteSearchClientBuySell ClientBuySell { get; set; } = FXDealQuoteSearchClientBuySell.Any;
    public string CurrencyCode { get; set; } = string.Empty;
    public decimal AmountMin { get; set; } = decimal.Zero;
    public decimal AmountMax { get; set; } = decimal.Zero;
    public FXDealQuoteSearchDateType DateType { get; set; } = FXDealQuoteSearchDateType.Value;
    public string DateMin { get; set; } = string.Empty;
    public string DateMax { get; set; } = string.Empty;
}

public enum FXDealQuoteSearchDateType
{
    [EnumMember]
    Deal = 0,
    [EnumMember]
    Value = 1
}

public enum FXDealQuoteSearchClientBuySell
{
    [EnumMember]
    Any = 0,
    [EnumMember]
    Buy = 1,
    [EnumMember]
    Sell = 2
}

public class FXDealQuoteSearchResponse : DTOResponseBase
{
    public int RecordCount { get; set; } = 0;
    public int TotalRecords { get; set; } = 0;
    public List<FXDealQuoteSearchData> Quotes { get; set; } = new List<FXDealQuoteSearchData>();
}

public class FXDealQuoteSearchData
{
    public Guid FXDealQuoteId { get; set; } 
    public string FXDealQuoteReference { get; set; }
    public Guid? BookedFXDealId { get; set; } 
    public string? BookedFXDealReference { get; set; } 
    public int RateFeedProviderId { get; set; }
    public string QuoteProviderQuoteId { get; set; }
    public string QuoteProviderName { get; set; }
    public string BranchName { get; set; }
    public string BranchCode { get; set; }
    public Guid QuotedForCustomerId { get; set; }
    public string QuotedForCustomerName { get; set; }
    public int FXDealTypeId { get; set; }
    public string FXDealTypeName { get; set; }
    public string FXDealQuoteType { get; set; }
    public string FXDealQuoteTypeName { get; set; }
    public bool IsBooked { get; set; } = false;
    public string QuoteTime { get; set; }
    public string ExpirationTime { get; set; }
    public string DealDate { get; set; }
    public Decimal BuyAmount { get; set; }
    public string BuyCurrencyCode { get; set; }
    public int BuyCurrencyScale { get; set; }
    public string BuyAmountTextBare { get; set; }
    public string BuyAmountTextWithCurrencyCode { get; set; }
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
}