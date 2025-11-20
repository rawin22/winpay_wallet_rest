using System.ComponentModel;
using System.Runtime.Serialization;

namespace GPWebApi.DTO;

public class IncomingPaymentSearchRequest
{
    [DefaultValue("0")]
    public int PageIndex { get; set; } = 0;
    [DefaultValue("25")]
    public int PageSize { get; set; } = 0;
    public IncomingPaymentSearchSortBy SortBy { get; set; } = IncomingPaymentSearchSortBy.None;
    public IncomingPaymentSearchSortDirection SortDirection { get; set; } = IncomingPaymentSearchSortDirection.Descending;
    public IncomingPaymentSearchOptions SearchOptions { get; set; } = new IncomingPaymentSearchOptions();
}

public enum IncomingPaymentSearchSortBy
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

public enum IncomingPaymentSearchSortDirection
{
    [EnumMember]
    Ascending = 0,
    [EnumMember]
    Descending = 1
}

public class IncomingPaymentSearchOptions
{
    public string IncomingPaymentReference { get; set; } = string.Empty;
    public IncomingPaymentStatus Status { get; set; } = IncomingPaymentStatus.All;
    public string CustomerName { get; set; } = string.Empty;
    public string BeneficiaryAccountNumber { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public decimal AmountMin { get; set; } = decimal.Zero;
    public decimal AmountMax { get; set; } = decimal.Zero;
    public string CurrencyCode { get; set; } = string.Empty;
    public string ValueDateMin { get; set; } = string.Empty;
    public string ValueDateMax { get; set; } = string.Empty;
}

public enum IncomingPaymentStatus
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

public class IncomingPaymentSearchResponse : DTOResponseBase
{
    public int RecordCount { get; set; } = 0;
    public int TotalRecords { get; set; } = 0;
    public List<IncomingPaymentSearchData> IncomingPayments { get; set; } = new List<IncomingPaymentSearchData>();
}

public class IncomingPaymentSearchData
{
    public string IncomingPaymentId { get; set; } = String.Empty;
    public string PaymentReference { get; set; } = string.Empty;
    public int PaymentSequenceNumber { get; set; } = 0;
    public int IncomingPaymentStatusTypeId { get; set; } = 0;
    public string IncomingPaymentStatusTypeName { get; set; } = String.Empty;
    public string ReceivingCustomerId { get; set; } = String.Empty;
    public string ReceivingCustomerName { get; set; } = String.Empty;
    public decimal Amount { get; set; } = Decimal.Zero;
    public string AmountCurrencyCode { get; set; } = String.Empty;
    public string AmountTextBare { get; set; } = String.Empty;
    public string AmountTextWithCurrencyCode { get; set; } = String.Empty;
    public string AmountTextBareSWIFT { get; set; } = string.Empty;
    public int AmountCurrencyScale { get; set; } = 0;
    public decimal FeeAmount { get; set; } = Decimal.Zero;
    public string FeeAmountCurrencyCode { get; set; } = String.Empty;
    public string FeeAmountTextBare { get; set; } = String.Empty;
    public string FeeAmountTextWithCurrencyCode { get; set; } = String.Empty;
    public int FeeAmountCurrencyScale { get; set; } = 0;
    public int WKYCStatusTypeId { get; set; } = 0;
    public string WKYCStatusTypeName { get; set; } = String.Empty;
    public string ProcessingBranchId { get; set; } = String.Empty;
    public string ProcessingBranchName { get; set; } = String.Empty;
    public string ProcessingBranchCode { get; set; } = String.Empty;
    public string ValueDate { get; set; } = String.Empty;
    public string CreatedTime { get; set; } = String.Empty;
    public string CreatedBy { get; set; } = String.Empty;
    public string CreatedByFullName { get; set; } = String.Empty;
    public string PostedTime { get; set; } = String.Empty;
    public string PostedBy { get; set; } = String.Empty;
    public string PostedByFullName { get; set; } = String.Empty;
    public bool IsDeleted { get; set; } = false;
}