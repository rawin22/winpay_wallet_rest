using GPWebApi.DTO;
using Wallet.Models;

namespace Wallet.Interfaces;

public interface IGetVerifiedMapperService
{
    FileAttachmentUpdateFileInfoRequest MapGetVerifiedToFileAttachmentUpdateRequest(GetVerifiedViewModel model);
    FileAttachmentUpdateFileInfoRequest MapGetVerifiedToFileAttachmentUpdateRequest(GetVerifiedViewModel model, FileAttachmentGetData fileData);
    FileAttachmentUpdateFileInfoRequest MapGetVerifiedToGetVerifiedFileAttachmentUpdateRequest(GetVerifiedViewModel model, FileAttachmentGetData fileData);
    FileAttachmentUpdateFileInfoRequest MapFileDataToUpdateRequest(FileAttachmentGetData fileData);
    CustomerUpdateRequest MapGetVerifiedToCustomerUpdateRequest(GetVerifiedViewModel model, CustomerGetData customerData);
}
