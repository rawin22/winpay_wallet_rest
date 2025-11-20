namespace GPWebApi.DTO;

public class CountryListGetResponse : DTOResponseBase
{
    public List<CountryData> Countries { get; set; } = new List<CountryData>();
}

public class CountryData
{
    public string CountryCode { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public bool IsIbanCountry { get; set; } = false;
}