namespace GPWebApi.DTO;

public class ShortenedUrlGetResponse
{
    public ShortenedUrlData ShortenedUrl { get; set; }
}

public class ShortenedUrlData
{
    public int Id { get; set; }
    public string ShortUrl { get { return $"https://{Domain}/{this.Key}"; } }
    public string LongUrl { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }

}
