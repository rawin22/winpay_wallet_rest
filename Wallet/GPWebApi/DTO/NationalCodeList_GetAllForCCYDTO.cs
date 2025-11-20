namespace GPWebApi.DTO;

public class NationalCodeListGetAllForCurrencyRequest
{
    public string CurrencyCode { get; set; }
}

public class NationalCodeListGetAllForCurrencyResponse : DTOResponseBase
{
    public List<NationalCodeData> NationalCodes { get; set; } 
}
