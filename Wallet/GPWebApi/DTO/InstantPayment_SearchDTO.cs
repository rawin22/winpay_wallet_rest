using System.ComponentModel;
using System.Runtime.Serialization;

namespace GPWebApi.DTO;

public class InstantPaymentSearchRequest
{
    [DefaultValue("0")]
    public int PageIndex { get; set; } = 0;
    [DefaultValue("25")]
    public int PageSize { get; set; } = 0;
    public string PaymentReference { get; set; } = string.Empty;
    public InstantPaymentStatus Status { get; set; } = InstantPaymentStatus.All;
    public string FromCustomerAlias { get; set; } = string.Empty;
    public string ToCustomerAlias { get; set; } = string.Empty;
    public string FromCustomerName { get; set; } = string.Empty;
    public string ToCustomerName { get; set; } = string.Empty;
    public decimal AmountMin { get; set; } = decimal.Zero;
    public decimal AmountMax { get; set; } = decimal.Zero;
    public string CurrencyCode { get; set; } = string.Empty;
    public string ValueDateMin { get; set; } = string.Empty;
    public string ValueDateMax { get; set; } = string.Empty;
    public InstantPaymentSearchSortBy SortBy { get; set; } = InstantPaymentSearchSortBy.None;
    public InstantPaymentSortDirection SortDirection { get; set; } = InstantPaymentSortDirection.Descending;
}

public enum InstantPaymentSearchSortBy
{
    [EnumMember]
    None = 0,
    [EnumMember]
    Reference = 1,
    [EnumMember]
    Status = 2,
    [EnumMember]
    Amount = 3,
    [EnumMember]
    CurrencyCode = 4,
    [EnumMember]
    ValueDate = 5,
    [EnumMember]
    CreatedTime = 6
}


public enum InstantPaymentSortDirection
{
    [EnumMember]
    Ascending = 0,
    [EnumMember]
    Descending = 1
}


public enum InstantPaymentStatus
{
    [EnumMember]
    All = 0,
    [EnumMember]
    Created = 1,
    [EnumMember]
    Posted = 2,
    [EnumMember]
    Voided = 3
}



public class InstantPaymentSearchResponse : DTOResponseBase
{
    public InstantPaymentSearchData Records { get; set; }
}

public class InstantPaymentSearchData : SearchRecords
{
    public List<InstantPaymentSearchRecord> Payments { get; set; }
}

public class InstantPaymentSearchRecord
{
    public Guid PaymentId { get; set; } 
    public string PaymentReference { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string FromCustomerAlias { get; set; } = string.Empty;
    public string ToCustomerAlias { get; set; } = string.Empty;
    public string ToCustomerName { get; set; } = string.Empty;
    public int PaymentTypeId { get; set; } = 1;
    public string PaymentTypeName { get; set; } = string.Empty;
    public string FromCustomerName { get; set; } = string.Empty;
    public decimal Amount { get; set; } = decimal.Zero;
    public string AmountTextBare { get; set; } = String.Empty;
    public string AmountTextWithCommas { get; set; } = String.Empty;
    public string AmountTextWithCurrencyCode { get; set; } = String.Empty;
    public string AmountTextWithCommasAndCurrencyCode { get; set; } = String.Empty;
    public string AmountTextBareSWIFT { get; set; } = String.Empty;
    public int AmountCurrencyScale { get; set; } = 0;
    public string CurrencyCode { get; set; } = string.Empty;
    public string ValueDate { get; set; } = String.Empty;
    public string CreatedTime { get; set; } = String.Empty;
    public string PostedTime { get; set; } = String.Empty;
    public string ExternalReference { get; set; } = string.Empty;
}

