namespace GPWebApi.DTO;

public class UserAccessRightUpdateRequest
{
    public Guid UserId { get; set; }
    public int AccessRightId { get; set; }
    public long LimitAmount { get; set; }
    public bool CanOverrideDualControl { get; set; }
}

public class UserAccessRightUpdateResponse : DTOResponseBase
{
}
