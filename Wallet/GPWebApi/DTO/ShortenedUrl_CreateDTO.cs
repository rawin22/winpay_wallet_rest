namespace GPWebApi.DTO;

public class ShortenedUrlPostRequest
{
    public string LongUrl { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
}

