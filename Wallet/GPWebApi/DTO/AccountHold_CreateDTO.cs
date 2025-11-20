namespace GPWebApi.DTO;

public class AccountHoldCreateRequest
{
    public Guid CustomerId { get; set; }
    public decimal? Amount { get; set; } = decimal.Zero;
    public string? AmountCurrencyCode { get; set; } = string.Empty;
    public string? ScheduledStartDate { get; set; } = string.Empty;
    public string? ScheduledReleaseDate { get; set; } = string.Empty;
    public bool IndefiniteHold { get; set; } = false;
    public string? Memo { get; set; } = string.Empty;
}

public class AccountHoldCreateResponse : DTOResponseBase
{
    public AccountHoldCreateData AccountHold { get; set; }
}

public class AccountHoldCreateData
{
    public Guid AccountHoldId { get; set; } 
    public string AccountHoldReference { get; set; } = string.Empty;
}

