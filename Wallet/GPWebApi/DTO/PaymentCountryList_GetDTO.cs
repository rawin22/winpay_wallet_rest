namespace GPWebApi.DTO;

public class PaymentCountryListGetResponse : DTOResponseBase
{
    public List<PaymentCountryData> PaymentCountries { get; set; } = new List<PaymentCountryData>();
}

public class PaymentCountryData
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
    //public CountryBlockStatusType BlockStatus { get; set; } = CountryBlockStatusType.UnBlocked;
    public int BlockStatus { get; set; } = 0;
}

//public enum CountryBlockStatusType
//{
//    UnBlocked = 0,
//    PartiallyBlocked = 1,
//    Blocked = 2
//}