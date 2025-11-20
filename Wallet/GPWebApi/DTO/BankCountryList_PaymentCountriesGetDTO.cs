namespace GPWebApi.DTO;

public class BankCountryListGetPaymentCountriesResponse : DTOResponseBase
{
    public List<BankCountryData> Countries { get; set; } = new List<BankCountryData>();
}

public class BankCountryData
{
    public string CountryCode { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public bool IsIbanCountry { get; set; } = false;
    public int SortOrder { get; set; } = 0;
    public bool IsEnabled { get; set; } = false;
    public bool IsBlocked { get; set; } = false;
    public bool IsCustomerPostalCodeRequired { get; set; } = false;
    public string Memo { get; set; } = string.Empty;
    public string DefaultCurrencyCode { get; set; } = string.Empty;
    public int BlockStatus { get; set; } = 0;
}