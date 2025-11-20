namespace GPWebApi.DTO;

public class UserUsernameChangeRequest
{
    public Guid UserId { get; set; }
    public string NewUsername { get; set; }
}

public class UserUsernameChangeResponse : DTOResponseBase
{
}
