namespace GPWebApi.DTO;

public class CustomerLiquidationPreferenceUpdateAllRequest
{
    public Guid CustomerId { get; set; }
    public string CurrencyList { get; set; } = String.Empty;
}

public class CustomerLiquidationPreferenceUpdateAllResponse : DTOResponseBase
{
}