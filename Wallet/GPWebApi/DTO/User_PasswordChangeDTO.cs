namespace GPWebApi.DTO;

public class UserPasswordChangeRequest
{
    public Guid UserId { get; set; }    
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}

public class UserPasswordChangeResponse : DTOResponseBase
{
}
