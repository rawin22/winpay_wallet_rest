namespace GPWebApi.DTO;

public class VerifiedLinkGetRequest
{
    public Guid VerifiedLinkId { get; set; }
}

public class VerifiedLinkGetResponse : DTOResponseBase
{
    public VerifiedLinkData VerifiedLink { get; set; }
}

public class VerifiedLinkData
{
    public Guid VerifiedLinkId { get; set; }
    public string VerifiedLinkReference { get; set; }
    public int VerifiedLinkSequenceNumber { get; set; }
    public string VerifiedLinkName { get; set; }
    public Guid BankId { get; set; }
    public Guid BranchId { get; set; }
    public string BranchName { get; set; }
    public string BranchCountryCode { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string EnglishCustomerName { get; set; }
    public int VerifiedLinkTypeId { get; set; }
    public string VerifiedLinkTypeName { get; set; }
    public int VerifiedLinkStatusTypeId { get; set; }
    public string VerifiedLinkStatusTypeName { get; set; }
    public string GroupName { get; set; }
    public CurrencyAmountResponse? Amount { get; set; }
    public Guid? ClientId { get; set; }
    public int MinimumWKYCLevel { get; set; }
    public string Message { get; set; }
    public string PublicMessage { get; set; }  
    public string BlockchainMessage { get; set; }   
    //public byte[]? BlockchainMessageImage { get; set; }
    public string SharedWithName { get; set; }
    public string WebsiteUrl { get; set; }
    public string VerifiedLinkUrl { get; set; }
    public string VerifiedLinkShortUrl { get; set; }
    //public string VerifiedLinkShortUrlQRCode { get; set; }
    public string SelectedAccountAlias { get; set; }
    public bool ShareAccountAlias { get; set; }
    public bool ShareBirthCity { get; set; }
    public bool ShareBirthCountry { get; set; }
    public bool ShareBirthDate { get; set; }
    public bool ShareFirstName { get; set; }
    public bool ShareMiddleName { get; set; }
    public bool ShareLastName { get; set; }
    public bool ShareGlobalFirstName { get; set; }
    public bool ShareGlobalMiddleName { get; set; }
    public bool ShareGlobalLastName { get; set; }
    public bool ShareGender { get; set; }
    public bool ShareNationality { get; set; }
    public bool ShareSuffix { get; set; }
    public bool ShareIdExpirationDate { get; set; }
    public bool ShareIdNumber { get; set; }
    public bool ShareIdType { get; set; }
    public bool ShareIdFront { get; set; }
    public bool ShareIdBack { get; set; }
    public bool ShareSelfie { get; set; }
    public VerifiedLinkTemplateFieldList Fields { get; set; }
    public VerifiedLinkTemplateFieldValueList FieldValues { get; set; }
    public Dictionary<string, string>? Properties { get; set; }
    public string AdditionalData { get; set; }
    public string NFTReference { get; set; }
    public string NFTChain { get; set; }
    public Guid? VerifiedLinkTemplateId { get; set; }
    public string VerifiedLinkTemplateName { get; set; }
    public DateTime? AvailableTime { get; set; }
    public DateTime? ExpirationTime { get; set; }
    public DateTime CreatedTime { get; set; }
    public Guid CreatedBy { get; set; }
    public string CreatedByName { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedTime { get; set; }
    public Guid? DeletedBy { get; set; }
    public string DeletedByName { get; set; }
    public string Timestamp { get; set; }
}

