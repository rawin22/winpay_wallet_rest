namespace GPWebApi.DTO;

public class UserPasswordResetRequest
{
    public Guid? UserId { get; set; }
    public bool SendEmail { get; set; }
}

public class UserPasswordResetResponse : DTOResponseBase
{
    public string NewPassword { get; set; } 
}
