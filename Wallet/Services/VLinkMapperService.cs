using GPWebApi.DTO;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Wallet.Helper;
using Wallet.Interfaces;
using Wallet.Models;
using static System.Net.WebRequestMethods;

namespace Wallet.Services;

public class VLinkMapperService : IVLinkMapperService
{
	private readonly ILogger<VLinkMapperService> _logger;

	public VLinkMapperService(ILogger<VLinkMapperService> logger)
	{
		_logger = logger;
	}

	public AccessAvailability GetAccessAvailability(int number)
	{
		// Implement logic to return the corresponding enum based on the number
		// You can use switch-case, LINQ, or any other method to map the number to the enum
		switch (number)
		{
			case 0:
				return AccessAvailability.Public;
			case 1:
				return AccessAvailability.Member;
			case 2:
				return AccessAvailability.VerifiedMember;
			default:
				return AccessAvailability.Public;
		}
	}

	public VLinkViewModel MapVLinkDataToViewModel(VerifiedLinkData data)
	{
		return new VLinkViewModel
		{
			VerifiedLinkTypeId = data.VerifiedLinkTypeId,
			CustomerId = data.CustomerId,
			VerifiedLinkId = data.VerifiedLinkId,
			VerifiedLinkTemplateId = data.VerifiedLinkTemplateId,
			VerifiedLinkReference = data.VerifiedLinkReference,
			VerifiedLinkName = data.VerifiedLinkName,
			BankId = data.BankId,
			BranchId = data.BranchId,
			BranchName = data.BranchName,
			BranchCountryCode = data.BranchCountryCode,
			SharedWithName = data.SharedWithName,
			MinimumWKYCLevel = data.MinimumWKYCLevel,
			AvailableTime = data.AvailableTime,
			ExpirationTime = data.ExpirationTime,
			Amount = new()
			{
				Amount = data.Amount is null ? 0 : data.Amount.Amount,
				CurrencyCode = data.Amount is null ? "" : data.Amount.CurrencyCode,
			},
			WebsiteUrl = data.WebsiteUrl,
			PublicMessage = data.PublicMessage,
			Message = data.Message,
			ShareGender = data.ShareGender,
			ShareFirstName = data.ShareFirstName,
			ShareMiddleName = data.ShareMiddleName,
			ShareLastName = data.ShareLastName,
			ShareGlobalFirstName = data.ShareGlobalFirstName,
			ShareGlobalMiddleName = data.ShareGlobalMiddleName,
			ShareGlobalLastName = data.ShareGlobalLastName,
			ShareBirthDate = data.ShareBirthDate,
			ShareBirthCountry = data.ShareBirthCountry,
			ShareBirthCity = data.ShareBirthCity,
			ShareNationality = data.ShareNationality,
			ShareIdType = data.ShareIdType,
			ShareIdNumber = data.ShareIdNumber,
			ShareIdExpirationDate = data.ShareIdExpirationDate,
			ShareIdFront = data.ShareIdFront,
			ShareIdBack = data.ShareIdBack,
			ShareSelfie = data.ShareSelfie,
			VerifiedLinkUrl = data.VerifiedLinkUrl,
			VerifiedLinkShortUrl = data.VerifiedLinkShortUrl,
			GroupName = data.GroupName,
			AdditionalData = data.AdditionalData,
			Timestamp = data.Timestamp,
			SelectedAccountAlias = data.SelectedAccountAlias,
			ShareAccountAlias = data.ShareAccountAlias
		};
	}

	public VerifiedLinkUpdateRequest MapVLinkViewModelToUpdateRequest(VLinkViewModel model)
	{
		var request = new VerifiedLinkUpdateRequest
		{
			VerifiedLinkTypeId = model.VerifiedLinkTypeId,
			VerifiedLinkId = model.VerifiedLinkId,
			GroupName = model.GroupName,
			Amount = new()
			{
				Amount = model.Amount!.Amount,
				CurrencyCode = model.Amount!.CurrencyCode
			},
			MinimumWKYCLevel = model.MinimumWKYCLevel,
			PublicMessage = model.PublicMessage,
			Message = model.Message,
			SharedWithName = model.SharedWithName,
			WebsiteUrl = model.WebsiteUrl,
			VerifiedLinkUrl = model.VerifiedLinkUrl,
			VerifiedLinkShortUrl = model.VerifiedLinkShortUrl,
			ShareBirthCity = model.ShareBirthCity,
			ShareBirthCountry = model.ShareBirthCountry,
			ShareBirthDate = model.ShareBirthDate,
			ShareFirstName = model.ShareFirstName,
			ShareMiddleName = model.ShareMiddleName,
			ShareLastName = model.ShareLastName,
			ShareGlobalFirstName = model.ShareGlobalFirstName,
			ShareGlobalMiddleName = model.ShareGlobalMiddleName,
			ShareGlobalLastName = model.ShareGlobalLastName,
			ShareGender = model.ShareGender,
			ShareNationality = model.ShareNationality,
			ShareSuffix = model.ShareSuffix,
			ShareIdExpirationDate = model.ShareIdExpirationDate,
			ShareIdNumber = model.ShareIdNumber,
			ShareIdType = model.ShareIdType,
			ShareIdFront = model.ShareIdFront,
			ShareIdBack = model.ShareIdBack,
			ShareSelfie = model.ShareSelfie,
			AdditionalData = model.AdditionalData,
			VerifiedLinkName = model.VerifiedLinkName,
			SelectedAccountAlias = model.SelectedAccountAlias,
			ShareAccountAlias = model.ShareAccountAlias
		};

		if (model.AvailableTime is not null && model.AvailableTime.HasValue)
		{
			_logger.LogInformation("MapVLinkViewModelToUpdateRequest, AvailableTime is not null: {AvailableTime}", model.AvailableTime);
			request.AvailableTime = model.AvailableTime.Value;
		}
		else
		{
			_logger.LogInformation("MapVLinkViewModelToUpdateRequest, AvailableTime is null");
		}

		if (model.ExpirationTime is not null && model.ExpirationTime.HasValue)
		{
			_logger.LogInformation("MapVLinkViewModelToUpdateRequest, ExpirationTime is not null: {ExpirationTime}", model.ExpirationTime);
			request.ExpirationTime = model.ExpirationTime.Value;
		}
		else
		{
			_logger.LogInformation("MapVLinkViewModelToUpdateRequest, ExpirationTime is null");
		}

		Console.WriteLine($"MapVLinkViewModelToUpdateRequest, request: {JsonConvert.SerializeObject(request)}");

		return request;
	}

	public VerifiedLinkUpdateRequest MapVerifiedLinkDataToUpdateRequest(VerifiedLinkData data)
	{
		var request = new VerifiedLinkUpdateRequest
		{
			VerifiedLinkTypeId = data.VerifiedLinkTypeId,
			VerifiedLinkId = data.VerifiedLinkId,
			GroupName = data.GroupName,
			//Amount = new()
			//{
			//	Amount = data.Amount!.Amount,
			//	CurrencyCode = data.Amount!.CurrencyCode
			//},
			MinimumWKYCLevel = data.MinimumWKYCLevel,
			Message = data.Message,
			SharedWithName = data.SharedWithName,
			WebsiteUrl = data.WebsiteUrl,
			VerifiedLinkUrl = data.VerifiedLinkUrl,
			VerifiedLinkShortUrl = data.VerifiedLinkShortUrl,
			ShareBirthCity = data.ShareBirthCity,
			ShareBirthCountry = data.ShareBirthCountry,
			ShareBirthDate = data.ShareBirthDate,
			ShareFirstName = data.ShareFirstName,
			ShareMiddleName = data.ShareMiddleName,
			ShareLastName = data.ShareLastName,
			ShareGlobalFirstName = data.ShareGlobalFirstName,
			ShareGlobalMiddleName = data.ShareGlobalMiddleName,
			ShareGlobalLastName = data.ShareGlobalLastName,
			ShareGender = data.ShareGender,
			ShareNationality = data.ShareNationality,
			ShareSuffix = data.ShareSuffix,
			ShareIdExpirationDate = data.ShareIdExpirationDate,
			ShareIdNumber = data.ShareIdNumber,
			ShareIdType = data.ShareIdType,
			ShareIdFront = data.ShareIdFront,
			ShareIdBack = data.ShareIdBack,
			ShareSelfie = data.ShareSelfie,
			AdditionalData = data.AdditionalData,
			VerifiedLinkName = data.VerifiedLinkName,
			SelectedAccountAlias = data.SelectedAccountAlias,
			ShareAccountAlias = data.ShareAccountAlias,
		};

		if (data.Amount is not null)
		{
			request.Amount = new()
			{
				Amount = data.Amount!.Amount,
				CurrencyCode = data.Amount!.CurrencyCode
			};
		}
		else
		{
			request.Amount = new()
			{
				Amount = 0,
				CurrencyCode = "USD"
			};
		}

		if (data.AvailableTime.HasValue)
		{
			request.AvailableTime = data.AvailableTime.Value;
		}

		if (data.ExpirationTime.HasValue)
		{
			request.ExpirationTime = data.ExpirationTime.Value;
		}

		Console.WriteLine($"MapVerifiedLinkDataToUpdateRequest, request: {JsonConvert.SerializeObject(request)}");

		return request;
	}

	public VerifiedLinkCreateRequest MapVLinkViewModelToCreateRequest(VLinkViewModel model)
	{
		var request = new VerifiedLinkCreateRequest
		{
			CustomerId = model.CustomerId,
			GroupName = model.GroupName,
			Amount = new()
			{
				Amount = model.Amount.Amount,
				CurrencyCode = model.Amount.CurrencyCode,
			},
			MinimumWKYCLevel = model.MinimumWKYCLevel,
			PublicMessage = model.PublicMessage,
			Message = model.Message,
			SharedWithName = model.SharedWithName,
			WebsiteUrl = model.WebsiteUrl,
			VerifiedLinkUrl = model.VerifiedLinkUrl,
			VerifiedLinkShortUrl = model.VerifiedLinkShortUrl,
			ShareBirthCity = model.ShareBirthCity,
			ShareBirthCountry = model.ShareBirthCountry,
			ShareBirthDate = model.ShareBirthDate,
			ShareFirstName = model.ShareFirstName,
			ShareMiddleName = model.ShareMiddleName,
			ShareLastName = model.ShareLastName,
			ShareGlobalFirstName = model.ShareGlobalFirstName,
			ShareGlobalMiddleName = model.ShareGlobalMiddleName,
			ShareGlobalLastName = model.ShareGlobalLastName,
			ShareGender = model.ShareGender,
			ShareNationality = model.ShareNationality,
			ShareSuffix = model.ShareSuffix,
			ShareIdExpirationDate = model.ShareIdExpirationDate,
			ShareIdNumber = model.ShareIdNumber,
			ShareIdType = model.ShareIdType,
			ShareIdFront = model.ShareIdFront,
			ShareIdBack = model.ShareIdBack,
			ShareSelfie = model.ShareSelfie,
			VerifiedLinkTypeId = 1,
			VerifiedLinkName = model.VerifiedLinkName,
			AdditionalData = model.AdditionalData,
			SelectedAccountAlias = model.SelectedAccountAlias,
			ShareAccountAlias = model.ShareAccountAlias,
		};

		if (model.AvailableTime.HasValue)
		{
			request.AvailableTime = model.AvailableTime.Value;
		}

		if (model.ExpirationTime.HasValue)
		{
			request.ExpirationTime = model.ExpirationTime.Value;
		}

		Console.WriteLine($"MapVLinkViewModelToCreateRequest, request: {JsonConvert.SerializeObject(request)}");

		return request;
	}

	public VerifiedLinkCreateRequest MapGetVerifiedViewModelToCreateRequest(GetVerifiedViewModel model, CustomerGetData customerData)
	{
		var request = new VerifiedLinkCreateRequest
		{
			CustomerId = model.CustomerId,
			GroupName = string.Empty,
			Amount = new()
			{
				Amount = 0,
				CurrencyCode = string.Empty,
			},
			MinimumWKYCLevel = 2,
			Message = string.Empty,
			SharedWithName = string.Empty,
			WebsiteUrl = string.Empty,
			VerifiedLinkUrl = string.Empty,
			VerifiedLinkShortUrl = string.Empty,
			ShareBirthCity = true,
			ShareBirthCountry = true,
			ShareBirthDate = true,
			ShareFirstName = true,
			ShareMiddleName = true,
			ShareLastName = true,
			ShareGlobalFirstName = true,
			ShareGlobalMiddleName = true,
			ShareGlobalLastName = true,
			ShareGender = true,
			ShareNationality = true,
			ShareSuffix = false,
			ShareIdExpirationDate = true,
			ShareIdNumber = true,
			ShareIdType = true,
			ShareIdFront = true,
			ShareIdBack = true,
			ShareSelfie = true,
			VerifiedLinkTypeId = 4,
			VerifiedLinkName = $"Identity_Verification_{customerData.CustomerFirstName}_{customerData.CustomerLastName}_{DateTime.UtcNow.ToString("yyyyMMdd_HHmmss")}",
			AdditionalData = string.Empty
		};

		Console.WriteLine($"MapGetVerifiedViewModelToCreateRequest, request: {JsonConvert.SerializeObject(request)}");

		return request;
	}

	public VerifiedLinkCreateRequest MapGetVerifiedViewModelToCreateRequest(GetVerifiedViewModel model)
	{
		var request = new VerifiedLinkCreateRequest
		{
			CustomerId = model.CustomerId,
			GroupName = string.Empty,
			Amount = new()
			{
				Amount = 0,
				CurrencyCode = string.Empty,
			},
			MinimumWKYCLevel = 2,
			Message = string.Empty,
			SharedWithName = string.Empty,
			WebsiteUrl = string.Empty,
			VerifiedLinkUrl = string.Empty,
			VerifiedLinkShortUrl = string.Empty,
			ShareBirthCity = true,
			ShareBirthCountry = true,
			ShareBirthDate = true,
			ShareFirstName = true,
			ShareMiddleName = true,
			ShareLastName = true,
			ShareGlobalFirstName = true,
			ShareGlobalMiddleName = true,
			ShareGlobalLastName = true,
			ShareGender = true,
			ShareNationality = true,
			ShareSuffix = false,
			ShareIdExpirationDate = true,
			ShareIdNumber = true,
			ShareIdType = true,
			ShareIdFront = true,
			ShareIdBack = true,
			ShareSelfie = true,
			VerifiedLinkTypeId = 1,
			VerifiedLinkName = "Identity_Verification",
			AdditionalData = string.Empty
		};

		Console.WriteLine($"MapGetVerifiedViewModelToCreateRequest, request: {JsonConvert.SerializeObject(request)}");

		return request;
	}

	public VerifiedLinkDeleteRequest MapVLinkViewModelToDeleteRequest(VLinkViewModel model)
	{
		var request = new VerifiedLinkDeleteRequest
		{
			VerifiedLinkId = model.VerifiedLinkId,
			Timestamp = model.Timestamp
		};

		Console.WriteLine($"MapVLinkViewModelToDeleteRequest, request: {JsonConvert.SerializeObject(request)}");

		return request;
	}

	public VerifiedLinkDeleteRequest MapZKQRViewModelToDeleteRequest(ZKQRViewModel model)
	{
		var request = new VerifiedLinkDeleteRequest
		{
			VerifiedLinkId = model.VerifiedLinkId,
			Timestamp = model.Timestamp
		};

		Console.WriteLine($"MapVLinkViewModelToDeleteRequest, request: {JsonConvert.SerializeObject(request)}");

		return request;
	}

	public VerifiedLinkCreateRequest MapStealthIdModelToCreateRequest(StealthIdModel model, Guid customerId)
	{
		var request = new VerifiedLinkCreateRequest
		{
			CustomerId = customerId,
			GroupName = string.Empty,
			Amount = new()
			{
				Amount = 0,
				CurrencyCode = "",
			},
			MinimumWKYCLevel = model.AccessAvailable,
			Message = string.Empty,
			SharedWithName = string.Empty,
			WebsiteUrl = string.Empty,
			VerifiedLinkUrl = string.Empty,
			VerifiedLinkShortUrl = string.Empty,
			ShareBirthCity = false,
			ShareBirthCountry = false,
			ShareBirthDate = false,
			ShareFirstName = false,
			ShareMiddleName = false,
			ShareLastName = false,
			ShareGlobalFirstName = false,
			ShareGlobalMiddleName = false,
			ShareGlobalLastName = false,
			ShareGender = false,
			ShareNationality = false,
			ShareSuffix = false,
			ShareIdExpirationDate = false,
			ShareIdNumber = false,
			ShareIdType = false,
			ShareIdFront = false,
			ShareIdBack = false,
			ShareSelfie = false,
			AdditionalData = string.Empty,
			VerifiedLinkTypeId = 2, // Stealth ID
			VerifiedLinkName = model.StealthId
		};

		Console.WriteLine($"MapStealthIdModelToCreateRequest, request: {JsonConvert.SerializeObject(request)}");

		return request;
	}

	public VerifiedLinkUpdateRequest MapStealthIdModelToUpdateRequest(StealthIdModel model)
	{
		var request = new VerifiedLinkUpdateRequest
		{
			VerifiedLinkTypeId = 2, // Stealth ID
			VerifiedLinkId = model.Id,
			GroupName = string.Empty,
			VerifiedLinkName = model.StealthId,
			Amount = new()
			{
				Amount = 0,
				CurrencyCode = "",
			},
			MinimumWKYCLevel = model.AccessAvailable,
			Message = string.Empty,
			SharedWithName = string.Empty,
			WebsiteUrl = string.Empty,
			VerifiedLinkUrl = string.Empty,
			VerifiedLinkShortUrl = string.Empty,
			ShareBirthCity = false,
			ShareBirthCountry = false,
			ShareBirthDate = false,
			ShareFirstName = false,
			ShareMiddleName = false,
			ShareLastName = false,
			ShareGlobalFirstName = false,
			ShareGlobalMiddleName = false,
			ShareGlobalLastName = false,
			ShareGender = false,
			ShareNationality = false,
			ShareSuffix = false,
			ShareIdExpirationDate = false,
			ShareIdNumber = false,
			ShareIdType = false,
			ShareIdFront = false,
			ShareIdBack = false,
			ShareSelfie = false,
			AdditionalData = string.Empty,
		};

		Console.WriteLine($"MapStealthIdModelToUpdateRequest, request: {JsonConvert.SerializeObject(request)}");

		return request;
	}

	public VerifiedLinkDeleteRequest MapVLinkDataToDeleteRequest(VerifiedLinkData model)
	{
		var request = new VerifiedLinkDeleteRequest
		{
			VerifiedLinkId = model.VerifiedLinkId,
			Timestamp = model.Timestamp
		};

		Console.WriteLine($"MapVLinkViewModelToDeleteRequest, request: {JsonConvert.SerializeObject(request)}");

		return request;
	}

	public ZKQRViewModel MapVLinkDataToZKQRViewModel(VerifiedLinkData data)
	{
		return new ZKQRViewModel
		{
			VerifiedLinkTypeId = data.VerifiedLinkTypeId,
			CustomerId = data.CustomerId,
			VerifiedLinkId = data.VerifiedLinkId,
			VerifiedLinkTemplateId = data.VerifiedLinkTemplateId,
			VerifiedLinkName = data.VerifiedLinkName,
			VerifiedLinkReference = data.VerifiedLinkReference,
			SharedWithName = data.SharedWithName,
			WebsiteUrl = data.WebsiteUrl,
			ShareFirstName = data.ShareFirstName,
			ShareMiddleName = data.ShareMiddleName,
			ShareLastName = data.ShareLastName,
			ShareGlobalFirstName = data.ShareGlobalFirstName,
			ShareGlobalMiddleName = data.ShareGlobalMiddleName,
			ShareGlobalLastName = data.ShareGlobalLastName,
			VerifiedLinkUrl = data.VerifiedLinkUrl,
			VerifiedLinkShortUrl = data.VerifiedLinkShortUrl,
			Timestamp = data.Timestamp
		};
	}

	public VerifiedLinkCreateRequest MapZKQRViewModelToCreateRequest(ZKQRViewModel model, Guid customerId)
	{
		var request = new VerifiedLinkCreateRequest
		{
			CustomerId = customerId,
			GroupName = string.Empty,
			VerifiedLinkName = model.VerifiedLinkName,
			Amount = new()
			{
				Amount = 0,
				CurrencyCode = "",
			},
			MinimumWKYCLevel = 0,
			Message = string.Empty,
			SharedWithName = model.SharedWithName,
			WebsiteUrl = model.WebsiteUrl,
			VerifiedLinkUrl = string.Empty,
			VerifiedLinkShortUrl = string.Empty,
			ShareBirthCity = false,
			ShareBirthCountry = false,
			ShareBirthDate = false,
			ShareFirstName = model.ShareFirstName,
			ShareMiddleName = model.ShareMiddleName,
			ShareLastName = model.ShareLastName,
			ShareGlobalFirstName = model.ShareGlobalFirstName,
			ShareGlobalMiddleName = model.ShareGlobalMiddleName,
			ShareGlobalLastName = model.ShareGlobalLastName,
			ShareGender = false,
			ShareNationality = false,
			ShareSuffix = false,
			ShareIdExpirationDate = false,
			ShareIdNumber = false,
			ShareIdType = false,
			ShareIdFront = false,
			ShareIdBack = false,
			ShareSelfie = false,
			AvailableTime = DateTime.Now,
			ExpirationTime = DateTime.Now.AddYears(100),
			VerifiedLinkTypeId = 3, // ZK QR
			AdditionalData = string.Empty
		};

		Console.WriteLine($"MapZKQRViewModelToCreateRequest, request: {JsonConvert.SerializeObject(request)}");

		return request;
	}

	public VerifiedLinkUpdateRequest MapZKQRViewModelToUpdateRequest(ZKQRViewModel model)
	{
		var request = new VerifiedLinkUpdateRequest
		{
			VerifiedLinkTypeId = model.VerifiedLinkTypeId,
			VerifiedLinkId = model.VerifiedLinkId,
			GroupName = string.Empty,
			//CurrencyCode = string.Empty,
			VerifiedLinkName = model.VerifiedLinkName,
			Amount = new()
			{
				Amount = 0,
				CurrencyCode = "",
			},
			MinimumWKYCLevel = 0,
			Message = string.Empty,
			SharedWithName = model.SharedWithName,
			WebsiteUrl = model.WebsiteUrl,
			VerifiedLinkUrl = model.VerifiedLinkUrl,
			VerifiedLinkShortUrl = model.VerifiedLinkShortUrl,
			ShareBirthCity = false,
			ShareBirthCountry = false,
			ShareBirthDate = false,
			ShareFirstName = model.ShareFirstName,
			ShareMiddleName = model.ShareMiddleName,
			ShareLastName = model.ShareLastName,
			ShareGlobalFirstName = model.ShareGlobalFirstName,
			ShareGlobalMiddleName = model.ShareGlobalMiddleName,
			ShareGlobalLastName = model.ShareGlobalLastName,
			ShareGender = false,
			ShareNationality = false,
			ShareSuffix = false,
			ShareIdExpirationDate = false,
			ShareIdNumber = false,
			ShareIdType = false,
			ShareIdFront = false,
			ShareIdBack = false,
			ShareSelfie = false,
			AvailableTime = DateTime.Now,
			ExpirationTime = DateTime.Now.AddYears(100),
			AdditionalData = string.Empty,
		};

		Console.WriteLine($"MapZKQRViewModelToUpdateRequest, request: {JsonConvert.SerializeObject(request)}");

		return request;
	}

	public VerifyResultModel MapVLinkDataToVerifyResultModel(VerifiedLinkData data)
	{
		var request = new VerifyResultModel
		{
			VerifiedLinkId = data.VerifiedLinkId,
			VerifiedLinkTypeId = data.VerifiedLinkTypeId,
			VerifiedLinkReference = data.VerifiedLinkReference,
			VerifiedLinkName = data.VerifiedLinkName,
			CustomerId = data.CustomerId,
			CustomerName = data.CustomerName,
			EnglishCustomerName = data.EnglishCustomerName,
			MinimumWKYCLevel = data.MinimumWKYCLevel,
			PublicMessage = data.PublicMessage,
			Message = data.Message,
			SharedWithName = data.SharedWithName,
			WebsiteUrl = data.WebsiteUrl,
			VerifiedLinkUrl = data.VerifiedLinkUrl,
			VerifiedLinkShortUrl = data.VerifiedLinkShortUrl,
			ShareBirthCity = data.ShareBirthCity,
			ShareBirthCountry = data.ShareBirthCountry,
			ShareBirthDate = data.ShareBirthDate,
			ShareFirstName = data.ShareFirstName,
			ShareMiddleName = data.ShareMiddleName,
			ShareLastName = data.ShareLastName,
			ShareGlobalFirstName = data.ShareGlobalFirstName,
			ShareGlobalMiddleName = data.ShareGlobalMiddleName,
			ShareGlobalLastName = data.ShareGlobalLastName,
			ShareGender = data.ShareGender,
			ShareNationality = data.ShareNationality,
			ShareSuffix = data.ShareSuffix,
			ShareIdExpirationDate = data.ShareIdExpirationDate,
			ShareIdNumber = data.ShareIdNumber,
			ShareIdType = data.ShareIdType,
			ShareIdFront = data.ShareIdFront,
			ShareIdBack = data.ShareIdBack,
			ShareSelfie = data.ShareSelfie,
			AvailableTime = data.AvailableTime,
			ExpirationTime = data.ExpirationTime,
			AdditionalData = data.AdditionalData,
			SelectedAccountAlias = data.SelectedAccountAlias,
			ShareAccountAlias = data.ShareAccountAlias
		};

		if (data.Amount is not null)
		{
			request.Amount = new()
			{
				Amount = data.Amount!.Amount,
				AmountTextBare = data.Amount!.AmountText,
				AmountTextWithCurrencyCode = data.Amount!.AmountTextWithCCY,
				AmountTextWithCommas = data.Amount!.AmountTextWithCommas,
				AmountTextWithCommasAndCurrencyCode = data.Amount!.AmountTextWithCommasAndCCY
			};
		}

		Console.WriteLine($"MapVLinkDataToVerifyResultModel, request: {JsonConvert.SerializeObject(request)}");

		return request;
	}
}
