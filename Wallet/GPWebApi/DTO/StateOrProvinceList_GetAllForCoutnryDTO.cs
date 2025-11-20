namespace GPWebApi.DTO;

public class StateOrProvinceListGetRequest
{
    public string CountryCode = String.Empty;
}

public class StateOrProvinceListGetResponse : DTOResponseBase
{
    public List<StateOrProvinceData> StatesOrProvinces { get; set; } = new List<StateOrProvinceData>();
}

public class StateOrProvinceData
{
    public string StateCode { get; set; } = string.Empty;
    public string StateName { get; set; } = string.Empty;
}