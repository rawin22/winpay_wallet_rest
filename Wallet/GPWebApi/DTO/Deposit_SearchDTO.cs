using System.ComponentModel;
using System.Runtime.Serialization;

namespace GPWebApi.DTO;

public class DepositSearchRequest
{
    [DefaultValue("0")]
    public int PageIndex { get; set; } = 0;
    [DefaultValue("25")]
    public int PageSize { get; set; } = 0;
    public string DepositReference { get; set; } = string.Empty;
    public DepositStatus Status { get; set; } = DepositStatus.All;
    public string CustomerName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public decimal AmountMin { get; set; } = decimal.Zero;
    public decimal AmountMax { get; set; } = decimal.Zero;
    public string CurrencyCode { get; set; } = string.Empty;
    public string ValueDateMin { get; set; } = string.Empty;
    public string ValueDateMax { get; set; } = string.Empty;
    public bool? IsTransmitted { get; set; } 
    public DepositSearchSortBy SortBy { get; set; } = DepositSearchSortBy.None;
    public DepositSearchSortDirection SortDirection { get; set; } = DepositSearchSortDirection.Descending;
}

public enum DepositSearchSortBy
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

public enum DepositSearchSortDirection
{
    [EnumMember]
    Ascending = 0,
    [EnumMember]
    Descending = 1
}


public enum DepositStatus
{
    [EnumMember]
    All = 0,
    [EnumMember]
    Created = 1,
    [EnumMember]
    Submitted = 2,
    [EnumMember]
    FundsApproved = 3,
    [EnumMember]
    Verified = 4,
    [EnumMember]
    Released = 5,
    [EnumMember]
    Posted = 6,
    [EnumMember]
    Voided = 7
}

public class DepositSearchResponse : DTOResponseBase
{
    public DepositSearchData Records { get; set; }
}

public class DepositSearchData : SearchRecords
{
    public List<DepositSearchRecord> Deposits { get; set; }
}

public class DepositSearchRecord
{
    public Guid DepositId { get; set; }
    public string DepositReference { get; set; } = string.Empty;
    public Guid? FXDealId { get; set; }
    public string FXDealReference { get; set; } = string.Empty;
    public int DepositStatusTypeId { get; set; } = 0;
    public string DepositStatusTypeName { get; set; } = string.Empty;
    public int WKYCStatusTypeId { get; set; } = 0;
    public string WKYCStatusTypeName { get; set; } = String.Empty;
    public Guid ProcessingBranchId { get; set; } 
    public string ProcessingBranchName { get; set; } = string.Empty;
    public Guid CustomerId { get; set; } 
    public string CustomerName { get; set; } = string.Empty;
    public decimal Amount { get; set; } = decimal.Zero;
    public string AmountCurrencyCode { get; set; } = string.Empty;
    public string AmountTextBare { get; set; } = string.Empty;
    public string AmountTextWithCurrencyCode { get; set; } = string.Empty;
    public string AmountTextBareSWIFT { get; set; } = string.Empty;
    public decimal FeeAmount { get; set; } = decimal.Zero;
    public string FeeAmountCurrencyCode { get; set; } = string.Empty;
    public string FeeAmountTextBare { get; set; } = string.Empty;
    public string FeeAmountTextWithCurrencyCode { get; set; } = string.Empty;
    public string ValueDate { get; set; } = String.Empty;
    public string CreatedTime { get; set; } = String.Empty;
    public string CreatedBy { get; set; } = String.Empty;
    public string CreatedByFullName { get; set; } = String.Empty;
    public string? SubmittedTime { get; set; } 
    public Guid? SubmittedBy { get; set; } 
    public string? SubmittedByFullName { get; set; } 
    public string? ApprovedTime { get; set; } 
    public Guid? ApprovedBy { get; set; } 
    public string? ApprovedByFullName { get; set; } 
    public string? VerifiedTime { get; set; }
    public Guid? VerifiedBy { get; set; } 
    public string? VerifiedByFullName { get; set; } 
    public string? ReleasedTime { get; set; }
    public Guid? ReleasedBy { get; set; } 
    public string? ReleasedByFullName { get; set; } 
    public string? DeletedTime { get; set; } 
    public Guid? DeletedBy { get; set; } 
    public string? DeletedByFullName { get; set; } 
    public bool IsDeleted { get; set; } = false;
 }