namespace GPWebApi.DTO;

public class NationalCodeListGetAllResponse : DTOResponseBase
{
    public List<NationalCodeData> NationalCodes { get; set; } 
}

public class NationalCodeData
{
    public string NationalCode { get; set; } 
    public string NationalCodeName { get; set; }
    public string NationalCodeDescription { get; set; }
}