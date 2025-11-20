using GPLibrary.DataObjects;

namespace Wallet.Models;

public class VerifyModel
{
	public string ThirdPartyId { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public DateOnly? DateOfBirth { get; set; }
	public string CountryCode { get; set; }
}

public class VerifyResultAmountModel
{
	public decimal Amount { get; set; }
	public string AmountTextBare { get; set; }
	public string AmountTextWithCurrencyCode { get; set; }
	public string AmountTextWithCommas { get; set; }
	public string AmountTextWithCommasAndCurrencyCode { get; set; }
}

public class VerifyResultModel
{
	public int VerifiedLinkTypeId { get; set; }
	public Guid VerifiedLinkId { get; set; }
	public string VerifiedLinkReference { get; set; }
	public string VerifiedLinkName { get; set; }
	public Guid CustomerId { get; set; }
	public string CustomerName { get; set; }
	public string EnglishCustomerName { get; set; }
	public int CurrencyId { get; set; }
	public string CurrencyCode { get; set; }
	public string ThirdPartyId { get; set; }
	public VerifyResultAmountModel? Amount { get; set; }
	public int MinimumWKYCLevel { get; set; }
	public string PublicMessage { get; set; }
	public int? OwnerKYCLevel { get; set; }
	public int? CallerKYCLevel { get; set; }
	public string Message { get; set; }
	public string SharedWithName { get; set; }
	public string WebsiteUrl { get; set; }
	public string VerifiedLinkUrl { get; set; }
	public string VerifiedLinkShortUrl { get; set; }
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
	public DateTime? AvailableTime { get; set; }
	public DateTime? ExpirationTime { get; set; }
	public DateTime? CreatedTime { get; set; }
	public Guid CreatedBy { get; set; }
	public string CreatedByName { get; set; }
	public bool IsDeleted { get; set; }
	public DateTime? DeletedTime { get; set; }
	public Guid? DeletedBy { get; set; }
	public string DeletedByName { get; set; }
	public string AdditionalData { get; set; }
	public string Timestamp { get; set; }
	public string BirthCity { get; set; }
	public string BirthCountry { get; set; }
	public string BirthDate { get; set; }
	public string FirstName { get; set; }
	public string MiddleName { get; set; }
	public string LastName { get; set; }
	public string GlobalFirstName { get; set; }
	public string GlobalMiddleName { get; set; }
	public string GlobalLastName { get; set; }
	public string Gender { get; set; }
	public string Nationality { get; set; }
	public string Suffix { get; set; }
	public string IdExpirationDate { get; set; }
	public string IdNumber { get; set; }
	public string IdType { get; set; }
	public string IdFrontFileData { get; set; }
	public string IdBackFileData { get; set; }
	public string SelfieFileData { get; set; }
	public string SelectedAccountAlias { get; set; }
	public bool ShareAccountAlias { get; set; }
}