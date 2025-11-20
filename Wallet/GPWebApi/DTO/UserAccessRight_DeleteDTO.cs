namespace GPWebApi.DTO;

public class UserAccessRightDeleteRequest
{
    public Guid UserId { get; set; }
    public int AccessRightId { get; set; }
}

public class UserAccessRightDeleteResponse : DTOResponseBase
{
}
