namespace GPWebApi.DTO;

public class DepositCreateRequest
{
    public Guid CustomerId { get; set; }
    public Guid FXDealId { get; set; }
    public decimal Amount { get; set; }
    public string AmountCurrencyCode { get; set; } = string.Empty;
    public decimal FeeAmount { get; set; } 
    public string FeeAmountCurrencyCode { get; set; } = string.Empty;
    public string ValueDate { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string CustomerMemo { get; set; } = string.Empty;
    public string BankMemo { get; set; } = string.Empty;
}

public class DepositCreateResponse : DTOResponseBase
{
    public DepositCreateData? DepositInformation { get; set; } 
}

public class DepositCreateData
{
    public Guid DepositId { get; set; }
    public string DepositReference { get; set; } = string.Empty;
    public string Timestamp { get; set; } = string.Empty;
}

