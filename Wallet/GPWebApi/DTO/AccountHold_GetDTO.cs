namespace GPWebApi.DTO;

public class AccountHoldGetRequest
{
    public Guid AccountHoldId { get; set; }
}

public class AccountHoldGetResponse : DTOResponseBase
{
    public AccountHoldGetData AccountHold { get; set; }
}

public class AccountHoldGetData
{
    public Guid AccountHoldId { get; set; }
    public string AccountHoldReference { get; set; }
    public Guid BankId { get; set; }
    public int ApplicationId { get; set; } 
    public Guid BranchId { get; set; }
    public string BranchName { get; set; }
    public string BranchCode { get; set; }
    public decimal Amount { get; set; } 
    public string AmountTextBare { get; set; }
    public string AmountTextWithCurrencyCode { get; set; }
    public string AmountTextBareSWIFT { get; set; }
    public string AmountCurrencyCode { get; set; }
    public int AmountCurrencyScale { get; set; } 
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public Guid AccountId { get; set; }
    public string AccountNumber { get; set; }
    public string AccountName { get; set; }
    public string AccountFriendlyName { get; set; }
    public int AccountHoldStatusTypeId { get; set; } 
    public string AccountHoldStatusTypeName { get; set; }
    public string ScheduledStartDate { get; set; }
    public string ScheduledStartDateText { get; set; }
    public bool IsHoldIndefinite { get; set; } 
    public string ScheduledReleaseDate { get; set; }
    public string ScheduledReleaseDateText { get; set; }
    public string Memo { get; set; }
    public string? CreatedTime { get; set; }
    public Guid CreatedBy { get; set; }
    public string CreatedByName { get; set; }
    public string? AppliedTime { get; set; }
    public Guid? AppliedBy { get; set; }
    public string? AppliedByName { get; set; }
    public string? ReleasedTime { get; set; }

    public bool ReleasedBySystem = false;
    public Guid? ReleasedBy { get; set; }
    public string? ReleasedByName { get; set; }
    public bool IsDeleted { get; set; } 
    public string? DeletedTime { get; set; }
    public Guid? DeletedBy { get; set; }
    public string? DeletedByName { get; set; }
}
