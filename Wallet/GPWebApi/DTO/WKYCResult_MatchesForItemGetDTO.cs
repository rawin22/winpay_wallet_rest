namespace GPWebApi.DTO;

public class WKYCMatchesForItemGetResponse : DTOResponseBase
{
    public List<WKYCMatchData> Results { get; set; }
}

public class WKYCMatchData 
{
    public string WKCYMatchId { get; set; }
    public string CheckedByUserId { get; set; }
    public string CheckedByUserName { get; set; }
    public string CheckedTime { get; set; }
}