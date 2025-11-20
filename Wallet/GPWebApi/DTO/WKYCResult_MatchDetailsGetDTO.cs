namespace GPWebApi.DTO;

public class WKYCMatchDetailGetResponse : DTOResponseBase
{
    public List<WKYCMatchDetailGetData> Details { get; set; }
}

public class WKYCMatchDetailGetData 
{
    public string WKYCMatchDetailId { get; set; }
    public int FieldTypeId { get; set; }
    public int Score  { get; set; }
    public string WKYCListName { get; set; }
    public string FileName { get; set; }
    public string FileDate { get; set; }
    public string EntityName { get; set; }
    public string BestName { get; set; }
    public string Details { get; set; }
}