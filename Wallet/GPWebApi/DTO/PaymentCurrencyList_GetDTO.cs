namespace GPWebApi.DTO;

public class PaymentCurrencyListGetRequest
{
}

public class PaymentCurrencyListGetResponse : DTOResponseBase
{
    public List<CurrencyData> Currencies { get; set; } = null;
}

