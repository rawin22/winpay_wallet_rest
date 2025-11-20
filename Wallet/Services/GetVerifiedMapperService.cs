
using GPWebApi.DTO;
using Newtonsoft.Json;
using System.Text;
using Wallet.Interfaces;
using Wallet.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Wallet.Services;

public class GetVerifiedMapperService : IGetVerifiedMapperService
{
	public FileAttachmentUpdateFileInfoRequest MapGetDataToUpdateRequest(FileAttachmentGetData data)
	{
		return new FileAttachmentUpdateFileInfoRequest
		{
			FileAttachmentId = data.FileAttachmentId,
			GroupName = data.GroupName,
			FileAttachmentTypeId = data.FileAttachmentTypeId,
			ViewableByBanker = data.ViewableByBanker,
			ViewableByCustomer = data.ViewableByCustomer,
			DeletableByCustomer = data.DeletableByCustomer,
			Description = data.FileDescription
		};
	}

	private StringBuilder BuildFileAttachmentDescriptionString(GetVerifiedViewModel model)
	{
		StringBuilder builder = new StringBuilder();
		builder.Append("documentType: Proof of Identity, ");
		builder.Append($"country_of_issuance: {model.CountryOfIssuanceCode}, ");
		builder.Append($"id_type: {model.IdType}, ");
		builder.Append($"account/id_number: {model.IdNumber}, ");
		builder.Append($"issuer_name: {model.IssuerName}, ");
		if (model.IssuanceDate is not null)
		{
			builder.Append($"issuance_date: {model.IssuanceDate?.ToString("yyyy-MM-dd")}, ");
		}
		if (model.ExpirationDate is not null)
		{
			builder.Append($"expiry_date: {model.ExpirationDate?.ToString("yyyy-MM-dd")}");
		}

		return builder;
	}

	private string GenerateFileAttachmentDescription(GetVerifiedViewModel model)
	{
		var builder = BuildFileAttachmentDescriptionString(model);
		return builder.ToString();
	}

	private string GenerateGetVerifiedFileAttachmentDescription(GetVerifiedViewModel model)
	{
		var builder = BuildFileAttachmentDescriptionString(model);
		builder.Append($", is_get_verified_requested: {true}");
		builder.Append($", get_verified_requested_by_user_id: {model.RequestedUserId}");
		builder.Append($", get_verified_requested_by_user_name: {model.RequestedUserName}");
		builder.Append($", get_verified_requested_by_name: {model.RequestedName}");
		builder.Append($", get_verified_requested_date: {DateTime.UtcNow.ToString("yyyy-MM-dd")}");
		builder.Append($", get_verified_vlink_id: {model.VLinkId}");

		return builder.ToString();
	}

	public string GenerateGetVerifiedFileAttachmentDescription(string requestedUserId, string requestedUserName, string requestedFullName, string vlinkId)
	{
		var builder = new StringBuilder();
		builder.Append($", is_get_verified_requested: {true}");
		builder.Append($", get_verified_requested_by_user_id: {requestedUserId}");
		builder.Append($", get_verified_requested_by_user_name: {requestedUserName}");
		builder.Append($", get_verified_requested_by_name: {requestedFullName}");
		builder.Append($", get_verified_requested_date: {DateTime.UtcNow.ToString("yyyy-MM-dd")}");
		builder.Append($", get_verified_vlink_id: {vlinkId}");

		return builder.ToString();
	}

	public CustomerUpdateRequest MapGetVerifiedToCustomerUpdateRequest(GetVerifiedViewModel model, CustomerGetData customerData)
	{
		throw new NotImplementedException();
	}

	public FileAttachmentUpdateFileInfoRequest MapGetVerifiedToFileAttachmentUpdateRequest(GetVerifiedViewModel model, FileAttachmentGetData fileData)
	{
		return new FileAttachmentUpdateFileInfoRequest
		{
			FileAttachmentId = model.FrontFileId,
			GroupName = fileData.GroupName,
			FileAttachmentTypeId = fileData.FileAttachmentTypeId,
			ViewableByBanker = fileData.ViewableByBanker,
			ViewableByCustomer = fileData.ViewableByCustomer,
			DeletableByCustomer = fileData.DeletableByCustomer,
			Description = GenerateFileAttachmentDescription(model)
		};
	}

	public FileAttachmentUpdateFileInfoRequest MapGetVerifiedToFileAttachmentUpdateRequest(GetVerifiedViewModel model)
	{
		return new FileAttachmentUpdateFileInfoRequest
		{
			FileAttachmentId = model.FrontFileId,
			//GroupName = model.GroupName,
			//FileAttachmentTypeId = model.FileAttachmentTypeId,
			//ViewableByBanker = model.ViewableByBanker,
			//ViewableByCustomer = model.ViewableByCustomer,
			//DeletableByCustomer = model.DeletableByCustomer,
			Description = GenerateFileAttachmentDescription(model)
		};
	}

	public FileAttachmentUpdateFileInfoRequest MapGetVerifiedToGetVerifiedFileAttachmentUpdateRequest(GetVerifiedViewModel model, FileAttachmentGetData fileData)
	{
		return new FileAttachmentUpdateFileInfoRequest
		{
			FileAttachmentId = fileData.FileAttachmentId,
			GroupName = fileData.GroupName,
			FileAttachmentTypeId = fileData.FileAttachmentTypeId,
			ViewableByBanker = fileData.ViewableByBanker,
			ViewableByCustomer = fileData.ViewableByCustomer,
			DeletableByCustomer = fileData.DeletableByCustomer,
			Description = GenerateGetVerifiedFileAttachmentDescription(model)
		};
	}

	public FileAttachmentUpdateFileInfoRequest MapFileDataToUpdateRequest(FileAttachmentGetData fileData)
	{
		return new FileAttachmentUpdateFileInfoRequest
		{
			FileAttachmentId = fileData.FileAttachmentId,
			GroupName = fileData.GroupName,
			FileAttachmentTypeId = fileData.FileAttachmentTypeId,
			ViewableByBanker = fileData.ViewableByBanker,
			ViewableByCustomer = fileData.ViewableByCustomer,
			DeletableByCustomer = fileData.DeletableByCustomer,
			Description = fileData.FileDescription
		};
	}
}
