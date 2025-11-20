using System.ComponentModel;
using System.Runtime.Serialization;

namespace GPWebApi.DTO;

public class FXDealSearchRequest
{
    [DefaultValue("0")]
    public int PageIndex { get; set; } = 0;
    [DefaultValue("25")]
    public int PageSize { get; set; } = 0;
    public FXDealSearchSortBy SortBy { get; set; } = FXDealSearchSortBy.None;
    public FXDealSearchSortDirection SortDirection { get; set; } = FXDealSearchSortDirection.Descending;
    public FXDealSearchOptions SearchOptions { get; set; } = new FXDealSearchOptions();
}

public enum FXDealSearchSortBy
{
    [EnumMember]
    None = 0,
    [EnumMember]
    DealReference = 1,
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
    BookedRate = 9,
    [EnumMember]
    MarketRate = 10,
    [EnumMember]
    ValueDate = 11,
    [EnumMember] 
    BookedTime = 12,
}

public enum FXDealSearchSortDirection
{
    [EnumMember]
    Ascending = 0,
    [EnumMember]
    Descending = 1
}

public class FXDealSearchOptions
{
    public string FXDealReference { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public FXDealSearchClientBuySell ClientBuySell { get; set; } = FXDealSearchClientBuySell.Any;
    public string CurrencyCode { get; set; } = string.Empty;
    public decimal AmountMin { get; set; } = decimal.Zero;
    public decimal AmountMax { get; set; } = decimal.Zero;
    public FXDealSearchDateType DateType { get; set; } = FXDealSearchDateType.Value;
    public string DateMin { get; set; } = string.Empty;
    public string DateMax { get; set; } = string.Empty;
}

public enum FXDealSearchDateType
{
    [EnumMember]
    Deal = 0,
    [EnumMember]
    Value = 1
}

public enum FXDealSearchClientBuySell
{
    [EnumMember]
    Any = 0,
    [EnumMember]
    Buy = 1,
    [EnumMember]
    Sell = 2
}

public class FXDealSearchResponse : DTOResponseBase
{
    public int RecordCount { get; set; } = 0;
    public int TotalRecords { get; set; } = 0;
    public List<FXDealSearchData> FXDeals { get; set; } = new List<FXDealSearchData>();
}

public class FXDealSearchData
{
    public string FXDealId { get; set; } = String.Empty;
    public string FXDealReference { get; set; } = String.Empty;
    public int RateFeedProviderId { get; set; } = 0;
    public string ProviderQuoteId { get; set; } = String.Empty;
    public string FXDealQuoteId { get; set; } = String.Empty;
    public string QuoteProviderName { get; set; } = String.Empty;
    public string BranchName { get; set; } = String.Empty;
    public string BranchCode { get; set; } = String.Empty;
    public string BookedForCustomerId { get; set; } = String.Empty;
    public string BookedForCustomerName { get; set; } = String.Empty;
    public int FXDealTypeId { get; set; } = 0;
    public string FXDealTypeName { get; set; } = String.Empty;
    public string BookedTime { get; set; } = String.Empty;
    public string DealDate { get; set; } = String.Empty;
    public Decimal BuyAmount { get; set; } = Decimal.Zero;
    public string BuyCurrencyCode { get; set; } = String.Empty;
    public int BuyCurrencyScale { get; set; } = 0;
    public string BuyAmountTextBare { get; set; } = String.Empty;
    public string BuyAmountTextWithCurrencyCode { get; set; } = String.Empty;
    public Decimal SellAmount { get; set; } = Decimal.Zero;
    public string SellCurrencyCode { get; set; } = String.Empty;
    public int SellCurrencyScale { get; set; } = 0;
    public string SellAmountTextBare { get; set; } = String.Empty;
    public string SellAmountTextWithCurrencyCode { get; set; } = String.Empty;
    public string AmountCurrencyCode { get; set; } = String.Empty;
    public string DealBaseCurrencyCode { get; set; } = String.Empty;
    public string DealCounterCurrencyCode { get; set; } = String.Empty;
    public string RateFormat { get; set; } = String.Empty;
    public Decimal BookedRate { get; set; } = Decimal.Zero;
    public int BookedRateScale { get; set; } = 0;
    public string BookedRateTextBare { get; set; } = String.Empty;
    public string BookedRateTextWithCurrencyCodes { get; set; } = String.Empty;
    public Decimal Bid { get; set; } = Decimal.Zero;
    public Decimal Ask { get; set; } = Decimal.Zero;
    public Decimal BuyCurrencyBid { get; set; } = Decimal.Zero;
    public Decimal BuyCurrencyAsk { get; set; } = Decimal.Zero;
    public Decimal SellCurrencyBid { get; set; } = Decimal.Zero;
    public Decimal SellCurrencyAsk { get; set; } = Decimal.Zero;
    public Decimal MarketSpotRate { get; set; } = Decimal.Zero;
    public string MarketSpotRateTextBare { get; set; } = String.Empty;
    public string MarketSpotRateTextWithCurrencyCodes { get; set; } = String.Empty;
    public Decimal MarketForwardRate { get; set; } = Decimal.Zero;
    public string MarketForwardRateTextBare { get; set; } = String.Empty;
    public string MarketForwardRateTextWithCurrencyCodes { get; set; } = String.Empty;
    public Decimal FinalSpotRate { get; set; } = Decimal.Zero;
    public Decimal ForwardPoints { get; set; } = Decimal.Zero;
    public Decimal FinalForwardRate { get; set; } = Decimal.Zero;
    public string FinalForwardRateTextBare { get; set; } = String.Empty;
    public string FinalForwardRateTextWithCurrencyCodes { get; set; } = String.Empty;
    public Decimal CoverRate { get; set; } = Decimal.Zero;
    public string CoverRateTextBare { get; set; } = String.Empty;
    public string CoverRateTextWithCurrencyCodes { get; set; } = String.Empty;
    public string SpotValueDate { get; set; } = String.Empty;
    public string WindowOpenDate { get; set; } = String.Empty;
    public string FinalValueDate { get; set; } = String.Empty;
    public bool IsDeleted { get; set; } = false;
}