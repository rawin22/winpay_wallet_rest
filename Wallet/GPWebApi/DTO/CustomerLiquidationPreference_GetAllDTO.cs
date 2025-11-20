namespace GPWebApi.DTO;

public class CustomerLiquidationPreferenceGetAllResponse : DTOResponseBase
{
    public List<LiquidationPreferenceData> Preferences { get; set; }
}

public class LiquidationPreferenceData
{
    public string CurrencyCode { get; set; }
    public bool IsActiveForAutoExchange { get; set; }
    public int LiquidationOrder { get; set; } = 0;
}

