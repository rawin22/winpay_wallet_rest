using Azure.Core;
using GPWebApi.DTO;

namespace Wallet.Interfaces;

public interface IVLinkServiceHelper
{
	Task<string> GetVLinkShortUrl(string longUrl);
	string GenerateVLinkLongUrl(string baseUrl, Guid vlinkId);
	string GenerateVLinkLongUrl(string baseUrl, string vlinkReference);
	string GenerateStealthIdLongUrl(string baseUrl, Guid stealthIdId);
	string GenerateZKQRLongUrl(string baseUrl, Guid zkqrId);
	Guid GetVLinkIdFromVLinkViewUrl(string url);
	string GenerateMemberKYCLevelText(int kycLevel);
	string GetVerifyIconFileName(int memberWKYCLevel);
	string GetVerifyIconAlt(int memberWKYCLevel);
	VerifiedLinkCreateRequest CheckVerifiedLinkCreateRequest(VerifiedLinkCreateRequest request);
	VerifiedLinkUpdateRequest CheckVerifiedLinkUpdateRequest(VerifiedLinkUpdateRequest request);
	string GenerateGetVerifiedRequestVLinkMessage(string requestedUserId);
	string GenerateGetVerifiedRequestVLinkMessage(string requestedUserId, string requestedUserName, string requestedName, string idCountryOfIssuance, DateTime idIssuanceDate, string idIssuerAuthority, DateTime idExpirationDate);
	string GenerateGetVerifiedRequestVLinkMessage(string requestedUserId, string requestedUserName, string requestedName, string idCountryOfIssuance, string idType, string idNumber, DateTime idIssuanceDate, string idIssuerAuthority, DateTime idExpirationDate);
	string GenerateIdentificationVerifiedVLinkMessage(string verifiedByUserId, string verifiedByUserName, string verifiedByName, DateTime verifiedDate);
	string GenerateSelfieVerifiedVLinkMessage(string verifiedByUserId, string verifiedByUserName, string verifiedByName, DateTime verifiedDate);
	Dictionary<string, string> ReadMessage(string message);
}