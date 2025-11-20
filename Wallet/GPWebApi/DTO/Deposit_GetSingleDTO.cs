namespace GPWebApi.DTO;

public class DepositGetSingleRequest
{
    public Guid DepositId { get; set; }
}

public class DepositGetSingleResponse : DTOResponseBase
{
    public DepositGetSingleData? Deposit { get; set; } 
}

public class DepositGetSingleData
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
    public int FundingMethodId { get; set; } = 0;
    public string FundingMethodName {  get; set; } = string.Empty;
    //public string FundingMethodDescription { get; set; } = string.Empty;
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
    public string Timestamp { get; set; } = string.Empty;
}

