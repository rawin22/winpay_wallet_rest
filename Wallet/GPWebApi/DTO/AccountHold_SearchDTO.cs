using System.ComponentModel;
using System.Runtime.Serialization;

namespace GPWebApi.DTO;

public class AccountHoldSearchRequest
{

    [DefaultValue("0")]
    public int PageIndex { get; set; } = 0;
    [DefaultValue("25")]
    public int PageSize { get; set; } = 0;
    public string AccountHoldReference { get; set; } = string.Empty;
    public AccountHoldStatus Status { get; set; } = AccountHoldStatus.All;
    public string CustomerName { get; set; } = string.Empty;
    public decimal AmountMin { get; set; } = decimal.Zero;
    public decimal AmountMax { get; set; } = decimal.Zero;
    public string CurrencyCode { get; set; } = string.Empty;
    public string ScheduledStartDate { get; set; } = string.Empty;
    public string ScheduledReleaseDate { get; set; } = string.Empty;
    public AccountHoldSearchSortBy SortBy { get; set; } = AccountHoldSearchSortBy.None;
    public AccountHoldSearchSortDirection SortDirection { get; set; } = AccountHoldSearchSortDirection.Descending;
}

public enum AccountHoldSearchSortBy
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
    CustomerName = 5,
    [EnumMember]
    StartDate = 6,
    [EnumMember]
    ReleaseDate = 7
}


public enum AccountHoldSearchSortDirection
{
    [EnumMember]
    Ascending = 0,
    [EnumMember]
    Descending = 1
}


public enum AccountHoldStatus
{
    [EnumMember]
    All = 0,
    [EnumMember]
    Created = 1,
    [EnumMember]
    Active = 2,
    [EnumMember]
    Released = 3,
    [EnumMember]
    Voided = 4
}



public class AccountHoldSearchResponse : DTOResponseBase
{
    public AccountHoldSearchData Records { get; set; }
}

public class AccountHoldSearchData : SearchRecords
{
    public List<AccountHoldSearchRecord> AccountHolds { get; set; }
}


public class AccountHoldSearchRecord
{
    public Guid AccountHoldId { get; set; }
    public string AccountHoldReference { get; set; }
    public Guid BankId { get; set;}
    public int ApplicationId { get; set; }
    public Guid BranchId { get; set;}
    public string BranchName { get; set;}
    public string BranchCode { get; set;}
    public decimal Amount { get; set; }
    public string AmountTextBare { get; set;}
    public string AmountTextWithCurrencyCode { get; set;}
    public string AmountTextBareSWIFT { get; set;}
    public string AmountCurrencyCode { get; set;}
    public int AmountCurrencyScale { get; set; }
    public Guid CustomerId { get; set;}
    public string CustomerName { get; set;}
    public Guid AccountId { get; set;}
    public string AccountNumber { get; set;}
    public string AccountName { get; set;}
    public string AccountFriendlyName { get; set;}
    public int AccountHoldStatusTypeId { get; set; }
    public string AccountHoldStatusTypeName { get; set;}
    public string ScheduledStartDate { get; set;}
    public string ScheduledStartDateText { get; set;}
    public bool IsHoldIndefinite { get; set; }
    public string ScheduledReleaseDate { get; set;}
    public string ScheduledReleaseDateText { get; set;}
    public string Memo { get; set;}
    public string CreatedTime { get; set;}
    public string CreatedBy { get; set;}
    public string CreatedByName { get; set;}
    public string? AppliedTime { get; set;}
    public Guid? AppliedBy { get; set;}
    public string? AppliedByName { get; set;}
    public string? ReleasedTime { get; set;}
    public bool ReleasedBySystem { get; set; }
    public Guid? ReleasedBy { get; set;}
    public string? ReleasedByName { get; set;}
    public bool IsDeleted { get; set; }
    public string? DeletedTime { get; set;}
    public Guid? DeletedBy { get; set;}
    public string? DeletedByName { get; set;}
}


