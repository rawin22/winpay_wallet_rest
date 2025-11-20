namespace GPWebApi.DTO;

public class VerifiedLinkTypeListGetResponse : DTOResponseBase
{
    public List<VerifiedLinkTypeData> VerifiedLinkTypes { get; set; } = new List<VerifiedLinkTypeData>();
}

public class VerifiedLinkTypeData
{
    public int VerifiedLinkTypeID { get; set; }
    public string VerifiedLinkTypeName { get; set; } = ""; 
}


