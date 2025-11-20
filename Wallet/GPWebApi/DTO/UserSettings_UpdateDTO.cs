namespace GPWebApi.DTO;

public class UserSettingsUpdateRequest
{
    public Guid UserId { get; set; } 
    public Guid OrganizationId { get; set; }
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public string Phone { get; set; } = String.Empty;
    public string Fax { get; set; } = String.Empty;
    public string EmailAddress { get; set; } = String.Empty;
    public string WKYCId { get; set; } = String.Empty;
    public string PreferredLanguage { get; set; } = String.Empty;
    public string CultureCode { get; set; } = String.Empty;
    public bool IsEnabled { get; set; }
    public bool IsLockedOut { get; set; }
    public bool UserMustChangePassword { get; set; }
}

public class UserSettingsUpdateResponse : DTOResponseBase
{
    public UserUpdateData User { get; set; }
}

public class UserUpdateData
{
    public Guid UserId { get; set; }
    public string Timestamp { get; set; }
}

