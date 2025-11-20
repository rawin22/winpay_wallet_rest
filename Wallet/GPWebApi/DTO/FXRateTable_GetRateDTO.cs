namespace GPWebApi.DTO;

public class FXRateTableGetRateRequest
{
    public string BaseCurrencyCode { get; set; } = string.Empty;
    public string QuoteCurrencyCode { get; set; } = string.Empty;
}

public class FXRateTableGetRateResponse : DTOResponseBase
{
    public FXTableRate? Rate { get; set; } = null;
}

public class FXTableRate
{
    public int Result { get; set; }
    public string Symbol { get; set; }
    public decimal Bid { get; set; }
    public decimal Ask { get; set; }
    public string BaseCurrencyCode { get; set; }
    public string QuoteCurrencyCode { get; set; }
    public decimal BaseCurrencyBid { get; set; }
    public decimal BaseCurrencyAsk { get; set; }
    public string BaseCurrencyRateTime { get; set; }
    public bool BaseCurrencyRateIsDirect { get; set; }
    public decimal QuoteCurrencyBid { get; set; }
    public decimal QuoteCurrencyAsk { get; set; }
    public string QuoteCurrencyRateTime { get; set; }
    public bool QuoteCurrencyRateIsDirect { get; set; }
    public string Provider { get; set; }
}
