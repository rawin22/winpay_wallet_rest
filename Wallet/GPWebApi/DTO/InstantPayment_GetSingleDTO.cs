
namespace GPWebApi.DTO;

public class InstantPaymentGetResponse : DTOResponseBase
{
    public InstantPaymentGetData Payment { get; set; }
}

public class InstantPaymentGetData 
{
    public Guid PaymentId { get; set; }
    public string PaymentReference { get; set; } = string.Empty;
    public string FromCustomerAlias { get; set; } = string.Empty;
    public string ToCustomerAlias { get; set; } = string.Empty;
    public string FromCustomerName { get; set; } = string.Empty;
    public Guid FromCustomerId { get; set; } 
    public string ToCustomerName { get; set; } = string.Empty;
    public Guid ToCustomerId { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public int PaymentStatusTypeId { get; set; } = 0;
    public string PaymentStatusTypeName { get; set; } = string.Empty;
    public int PaymentTypeId { get; set; } = 0;
    public string PaymentTypeName { get; set; } = string.Empty;
    public decimal Amount { get; set; } = decimal.Zero;
    public string AmountTextBare { get; set; } = String.Empty;
    public string AmountTextWithCommas { get; set; } = String.Empty;
    public string AmountTextWithCurrencyCode { get; set; } = String.Empty;
    public string AmountTextWithCommasAndCurrencyCode { get; set; } = String.Empty;
    public string AmountTextBareSWIFT { get; set; } = String.Empty;
    public int AmountCurrencyScale { get; set; } = 2;
    public string CurrencyCode { get; set; } = string.Empty;
    public string ValueDate { get; set; } = String.Empty;
    public string ProcessingBranchName { get; set; } = string.Empty;
    public string ProcessingBranchCode { get; set; } = string.Empty;
    public string CreatedTime { get; set; } = string.Empty;
    public string CreatedByName { get; set; } = string.Empty;
    public string? PostedTime { get; set; } 
    public string? PostedByName { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
    public string ReasonForPayment { get; set; } = string.Empty;
    public string ExternalReference { get; set; } = string.Empty;
    public string BankMemo { get; set; } = string.Empty;
}