using GPWebApi.DTO;
using Wallet.Models;

namespace Wallet.Interfaces;

public interface IFileAttachmentMapperService
{
	FileAttachmentUpdateFileInfoRequest MapGetDataToUpdateRequest(FileAttachmentGetData data);
}
