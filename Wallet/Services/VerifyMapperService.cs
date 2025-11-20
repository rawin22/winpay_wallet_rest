
using GPWebApi.DTO;
using Newtonsoft.Json;
using Wallet.Interfaces;
using Wallet.Models;

namespace Wallet.Services;

public class VerifyMapperService : IVerifyMapperService
{
	public VerifyGetRequest MapVerifyViewModelToGetRequest(VerifyModel model)
	{
		var request = new VerifyGetRequest
		{
			VerifiedLinkReference = model.ThirdPartyId,
			FirstName = model.FirstName,
			LastName = model.LastName,
			DateOfBirth = model.DateOfBirth.HasValue ? model.DateOfBirth!.Value.ToString("yyyy-MM-dd") : "",
			CountryCode = model.CountryCode
		};

		Console.WriteLine($"MapVerifyViewModelToGetRequest, request: {JsonConvert.SerializeObject(request)}");
		return request;
	}
}
