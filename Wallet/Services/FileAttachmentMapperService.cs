
using GPWebApi.DTO;
using Newtonsoft.Json;
using Wallet.Interfaces;
using Wallet.Models;

namespace Wallet.Services;

public class FileAttachmentMapperService : IFileAttachmentMapperService
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
}
