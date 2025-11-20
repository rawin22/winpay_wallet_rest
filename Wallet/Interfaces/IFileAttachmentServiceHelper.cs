using GPWebApi.DTO;
using Microsoft.AspNetCore.Components.Forms;
using Wallet.Models;

namespace Wallet.Interfaces;

public interface IFileAttachmentServiceHelper
{
	Task<List<FileAttachmentGetInfo>> GetMemberDocuments(string customerId);
	Task<FileAttachmentGetData?> GetFileAttachmentInfo(List<FileAttachmentGetInfo> files, string fileName);
	Task<byte[]?> GetFileAttachmentData(List<FileAttachmentGetInfo> files, string fileName);
	Task<byte[]?> GetFrontFileData(List<FileAttachmentGetInfo> files);
	Task<byte[]?> GetBackFileData(List<FileAttachmentGetInfo> files);
	Task<byte[]?> GetSelfieData(List<FileAttachmentGetInfo> files);
	Task<FileAttachmentGetData?> GetFrontFileInfo(List<FileAttachmentGetInfo> files);
	Task<FileAttachmentGetData?> GetBackFileInfo(List<FileAttachmentGetInfo> files);
	Task<FileAttachmentGetData?> GetSelfieFileInfo(List<FileAttachmentGetInfo> files);
	string ConvertToBase64(byte[] imageData);
	Dictionary<string, string> ReadFileAttachmentDescription(string fileDescription);

	FileAttachmentAddFileRequest GenerateFileAttachmentAddFileRequest(Guid parentObjectId, int parentObjectTypeId, string fileName, byte[] fileData, string description, bool isViewableByBanker, bool isViewableByCustomer, bool isDeletableByCustomer);
	FileAttachmentAddFileRequest GenerateVLinkFileAttachmentAddFileRequest(Guid vlinkId, string fileName, byte[] fileData, string description);
	Task<WkycFileAttachemenmtListResponse> GetVLinkDocuments(Guid vlinkId);

	Task<string> ConvertFileToBase64(IBrowserFile file);
	Task<byte[]> ConvertFileToBytes(IBrowserFile file);
	string ConvertBytesArrayToBase64(byte[] data);
	bool IsImage(string fileName);
	string CreateDownloadLink(byte[] fileBytes);
	string GenerateSelfieDocumentDescription();
}