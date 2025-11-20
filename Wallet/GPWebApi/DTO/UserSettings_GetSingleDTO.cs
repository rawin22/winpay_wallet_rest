namespace GPWebApi.DTO;

public class UserSettingsGetRequest
{
    public Guid? UserId { get; set; }
    public bool IncludeAccessRights { get; set; } = true;
}

public class UserSettingsGetResponse : DTOResponseBase
{
    public UserSettingsData UserSettings { get; set; }
}

public class UserSettingsData
{
    public Guid UserId { get; set; } 
    public string UserName { get; set; } 
    public Guid OrganizationId { get; set; } 
    public int OrganizationTypeId { get; set; } 
    public string OrganizationName { get; set; } 
    public Guid BankId { get; set; } 
    public Guid BranchId { get; set; }
    public string BranchName { get; set; }  
    public string BranchCountryCode { get; set; }
    public bool BelongsToWhiteLabelBranch { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Fax { get; set; }
    public string WKYCId { get; set; }
    public string PreferredLanguage { get; set; }
    public string CultureCode { get; set; }
    public int CultureId { get; set; }
    public string EmailAddress { get; set; }
    public string BaseCurrencyCode { get; set; }
    public string Theme { get; set; }
    public string PageTitle { get; set; }
    public bool IsLockedOut { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsCurrencyCalculatorEnabled { get; set; }
    public bool IsPaymentValueTypeEnabled { get; set; }
    public Guid? WhiteLabelProfileId { get; set; } 
    public string BaseCountryCode { get; set; }
    public Guid? LinkedAccessRightTemplateId { get; set; }
    public string? LinkedAccessRightTemplateName { get; set; } 
    public string CreatedTime { get; set; }
    public string LastActivityTime { get; set; }
    public string LastLoginTime { get; set; }
    public string LastLoginLocalTime { get; set; }
    public string LastLoginIPAddress { get; set; }
    public string LastPasswordChangedTime { get; set; }
    public string LastLockoutTime { get; set; }
    public int FailedPasswordAttemptCount { get; set; }
    public string FailedPasswordAttemptWindowStart { get; set; }
    public int FailedPasswordAnswerAttemptCount { get; set; }
    public string FailedPasswordAnswerAttemptWindowStart { get; set; }
    public string PasswordRegEx { get; set; }
    public string PasswordRegExMessage { get; set; }
    public int SessionTimeout { get; set; }
    public bool IsBankAutoCoverFeatureEnabled { get; set; }
    public bool IsBankIncomingPaymentEnabled { get; set; }
    public bool IsBankInstantPaymentFeatureEnabled { get; set; }
    public bool IsManageCustomWKYCListsFeatureEnabled { get; set; }
    public bool IsTradeFinanceFeatureEnabled { get; set; }
    public bool IsSWIFTMessageFeatureEnabled { get; set; }
    public bool IsACHBatchFeatureEnabled { get; set; }
    public bool IsFileAttachmentFeatureEnabled { get; set; }
    public bool IsTwoFactorAuthenticationFeatureEnabled { get; set; }
    public bool IsTwoFactorAuthenticationRequired { get; set; }
    public List<UserLicenseInfo>? Licenses { get; set; }

    // ACCESS RIGHTS LIST
    public List<AccessRightData> AccessRights { get; set; }

    public UIStyleInfo Branding { get; set; }
}

public class UserLicenseInfo
{
    public string LicenseKey { get; set; }
    public string LicenseType { get; set; }
    public string Status { get; set; }
    public string ExpiresOn { get; set; }
}


public class UIStyleInfo
{
    public string AppTheme { get; set; } = string.Empty;
    public string TimeZoneName { get; set; } = string.Empty;
    public string EmailProfileName { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public string AddressLine1 { get; set; } = string.Empty;
    public string AddressLine2 { get; set; } = string.Empty;
    public string AddressLine3 { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Fax { get; set; } = string.Empty;
    public string BankUrl { get; set; } = string.Empty;
    public string BankLoginUrl { get; set; } = string.Empty;
    public string LoginPageLogoUrl { get; set; } = string.Empty;
    public byte[]? LoginPageLogo { get; set; }
    public string EmailLogoUrl { get; set; } = string.Empty;
    public byte[]? ReportLogo { get; set; }
}

