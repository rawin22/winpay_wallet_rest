using GPWebApi.DTO;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Wallet.Interfaces;
using Wallet.Models;


namespace Wallet.Helper;

public class VLinkServiceHelper : IVLinkServiceHelper
{
	private readonly ILogger<VLinkServiceHelper> _logger;
	private readonly IUrlShortenerService _urlShortenerService;
	private readonly ITsgCoreServiceHelper _wkycServiceHelper;


	public VLinkServiceHelper(
		ILogger<VLinkServiceHelper> logger,
		IUrlShortenerService urlShortenerService,
		ITsgCoreServiceHelper wkycServiceHelper
		)
	{
		_urlShortenerService = urlShortenerService;
		_logger = logger;
		_wkycServiceHelper = wkycServiceHelper;
	}

	public async Task<string> GetVLinkShortUrl(string longUrl)
	{
		Console.WriteLine("VLinkServiceHelper");
		Console.WriteLine($"GetVLinkShortUrl, longUrl: {longUrl}");

		string shortUrl = string.Empty;
		if (!string.IsNullOrEmpty(longUrl))
		{
			var shortUrlResponse = await _urlShortenerService.CreateShortUrlAsync(longUrl);
			Console.WriteLine($"shortUrlResponse: {shortUrlResponse.ShortUrl}");
			shortUrl = shortUrlResponse.ShortUrl;
		}

		return shortUrl;
	}

	public string GenerateVLinkLongUrl(string baseUrl, Guid vlinkId)
	{
		return $"{baseUrl}vlink-view/{vlinkId}";
	}

	public string GenerateVLinkLongUrl(string baseUrl, string vlinkReference)
	{
		return $"{baseUrl}vl/{vlinkReference}";
	}

	public string GenerateStealthIdLongUrl(string baseUrl, Guid stealthIdId)
	{
		return $"{baseUrl}stealth-id-view/{stealthIdId}";
	}

	public string GenerateZKQRLongUrl(string baseUrl, Guid zkqrId)
	{
		return $"{baseUrl}zk-qr-view/{zkqrId}";
	}

	public Guid GetVLinkIdFromVLinkViewUrl(string url)
	{
		Console.WriteLine("url: " + url);
		var vlinkId = Guid.Empty;

		// Use regex to extract the GUID from the URL
		string pattern = @"/([a-fA-F\d]{8}-[a-fA-F\d]{4}-[a-fA-F\d]{4}-[a-fA-F\d]{4}-[a-fA-F\d]{12})$";
		Match match = Regex.Match(url, pattern);

		if (match.Success)
		{
			vlinkId = new Guid(match.Groups[1].Value);
			Console.WriteLine($"GetVLinkIdFromVLinkViewUrl, vlinkId: {vlinkId}");
		}
		else
		{
			Console.WriteLine("GUID not found in the URL.");
		}

		// Output the GUID
		Console.WriteLine($"GetVLinkIdFromVLinkViewUrl, vlinkId: {vlinkId}");

		return vlinkId;
	}

	private async Task<VerifiedLinkData> getVLinkDetail(Guid vlinkId)
	{
		var vlinkData = await _wkycServiceHelper.GetVerifiedLinkDetailsAsync(vlinkId.ToString());

		return vlinkData;
	}

	public string GenerateMemberKYCLevelText(int kycLevel)
	{
		string result = "Member Not Verified";

		if (kycLevel >= 2)
		{
			result = "Member Verified";
		}
		return result;
	}

	public string GetVerifyIconFileName(int memberWKYCLevel)
	{
		string verifyImageName = "kyc-not-verified.jpg";
		if (memberWKYCLevel >= 2)
		{
			verifyImageName = "kyc-verified.svg";
		}

		return verifyImageName;
	}

	public string GetVerifyIconAlt(int memberWKYCLevel)
	{
		string verifyImageAlt = "VLink Not Verified";
		if (memberWKYCLevel >= 2)
		{
			verifyImageAlt = "VLink Verified";
		}
		return verifyImageAlt;
	}

	public VerifiedLinkCreateRequest CheckVerifiedLinkCreateRequest(VerifiedLinkCreateRequest request)
	{
		// Iterate through properties using reflection
		PropertyInfo[] properties = request.GetType().GetProperties();
		foreach (PropertyInfo property in properties)
		{
			// Check if property type is string and value is null, then set it to empty string
			if (property.PropertyType == typeof(string) && property.GetValue(request) == null)
			{
				property.SetValue(request, "");
			}
		}

		return request;
	}

	public VerifiedLinkUpdateRequest CheckVerifiedLinkUpdateRequest(VerifiedLinkUpdateRequest request)
	{
		// Iterate through properties using reflection
		PropertyInfo[] properties = request.GetType().GetProperties();
		foreach (PropertyInfo property in properties)
		{
			// Check if property type is string and value is null, then set it to empty string
			if (property.PropertyType == typeof(string) && property.GetValue(request) == null)
			{
				property.SetValue(request, "");
			}
		}

		return request;
	}

	public string GenerateGetVerifiedRequestVLinkMessage(string requestedUserId)
	{
		StringBuilder builder = new StringBuilder();
		builder.Append($"is_get_verified_requested: {true}");
		builder.Append($", get_verified_requested_by_user_id: {requestedUserId}");
		builder.Append($", get_verified_requested_date: {DateTime.UtcNow.ToString("yyyy-MM-dd")}");

		return builder.ToString();
	}

	public string GenerateGetVerifiedRequestVLinkMessage(string requestedUserId, string requestedUserName, string requestedName, string idCountryOfIssuance, DateTime idIssuanceDate, string idIssuerAuthority, DateTime idExpirationDate)
	{
		StringBuilder builder = new StringBuilder();
		builder.Append($"id_country_of_issuance: {idCountryOfIssuance}");
		builder.Append($", id_issuance_date: {idIssuanceDate.ToString("yyyy-MM-dd")}");
		builder.Append($", id_issuer_authority: {idIssuerAuthority}");
		builder.Append($", id_expiration_date: {idExpirationDate.ToString("yyyy-MM-dd")}");
		builder.Append($", is_get_verified_requested: {true}");
		builder.Append($", get_verified_requested_by_user_id: {requestedUserId}");
		builder.Append($", get_verified_requested_by_user_name: {requestedUserName}");
		builder.Append($", get_verified_requested_by_name: {requestedName}");
		builder.Append($", get_verified_requested_date: {DateTime.UtcNow.ToString("yyyy-MM-dd")}");

		return builder.ToString();
	}

	public string GenerateGetVerifiedRequestVLinkMessage(string requestedUserId, string requestedUserName, string requestedName, string idCountryOfIssuance, string idType, string idNumber, DateTime idIssuanceDate, string idIssuerAuthority, DateTime idExpirationDate)
	{
		StringBuilder builder = new StringBuilder();
		builder.Append($"id_country_of_issuance: {idCountryOfIssuance}");
		builder.Append($", id_type: {idType}");
		builder.Append($", id_number: {idNumber}");
		builder.Append($", id_issuance_date: {idIssuanceDate.ToString("yyyy-MM-dd")}");
		builder.Append($", id_issuer_authority: {idIssuerAuthority}");
		builder.Append($", id_expiration_date: {idExpirationDate.ToString("yyyy-MM-dd")}");
		builder.Append($", is_get_verified_requested: {true}");
		builder.Append($", get_verified_requested_by_user_id: {requestedUserId}");
		builder.Append($", get_verified_requested_by_user_name: {requestedUserName}");
		builder.Append($", get_verified_requested_by_name: {requestedName}");
		builder.Append($", get_verified_requested_date: {DateTime.UtcNow.ToString("yyyy-MM-dd")}");

		return builder.ToString();
	}

	public string GenerateIdentificationVerifiedVLinkMessage(string verifiedByUserId, string verifiedByUserName, string verifiedByName, DateTime verifiedDate)
	{
		StringBuilder builder = new StringBuilder();
		builder.Append($", is_identification_verified: {true}");
		builder.Append($", identification_verified_by_user_id: {verifiedByUserId}");
		builder.Append($", identification_verified_by_user_name: {verifiedByUserName}");
		builder.Append($", identification_verified_by_name: {verifiedByName}");
		builder.Append($", identification_verified_date: {verifiedDate.ToString("yyyy-MM-dd")}");

		return builder.ToString();
	}

	public string GenerateSelfieVerifiedVLinkMessage(string verifiedByUserId, string verifiedByUserName, string verifiedByName, DateTime verifiedDate)
	{
		StringBuilder builder = new StringBuilder();
		builder.Append($", is_selfie_verified: {true}");
		builder.Append($", selfie_verified_by_user_id: {verifiedByUserId}");
		builder.Append($", selfie_verified_by_user_name: {verifiedByUserName}");
		builder.Append($", selfie_verified_by_name: {verifiedByName}");
		builder.Append($", selfie_verified_date: {verifiedDate.ToString("yyyy-MM-dd")}");

		return builder.ToString();
	}

	public Dictionary<string, string> ReadMessage(string message)
	{
		// Create a dictionary to store key-value pairs
		Dictionary<string, string> keyValueDict = new Dictionary<string, string>();
		if (message.Length > 10)
		{
			// Split the data string by commas to separate key-value pairs
			string[] keyValuePairs = message.Split(',');
			// Iterate through each key-value pair
			foreach (string pair in keyValuePairs)
			{
				if (pair.Length > 2)
				{
					// Split each pair by colon to separate key and value
					string[] keyValue = pair.Trim().Split(':');

					// Extract key and value
					string key = keyValue[0].Trim();
					string value = string.Empty;
					if (keyValue.Length > 1)
					{
						value = keyValue[1].Trim();
					}
					// Add key-value pair to dictionary
					keyValueDict[key] = value;
				}
			}
		}

		return keyValueDict;
	}
}
