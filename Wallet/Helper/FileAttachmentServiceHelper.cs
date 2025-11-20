using GPWebApi.DTO;
using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp;
using System.Text;
using Wallet.Interfaces;
using Wallet.Models;

namespace Wallet.Helper;

public class FileAttachmentServiceHelper : IFileAttachmentServiceHelper
{
	private readonly IConfiguration _configuration;
	private readonly ILogger<FileAttachmentServiceHelper> _logger;
	private readonly ITsgCoreServiceHelper _wkycServiceHelper;

	public FileAttachmentServiceHelper(
		ILogger<FileAttachmentServiceHelper> logger,
		ITsgCoreServiceHelper wkycServiceHelper,
		IConfiguration configuration
		)
	{
		_wkycServiceHelper = wkycServiceHelper;
		_logger = logger;
		_configuration = configuration;
	}

	public async Task<List<FileAttachmentGetInfo>> GetMemberDocuments(string customerId)
	{
		var fileList = await _wkycServiceHelper.FileAttachmentGetFileListForObjectAsync(customerId);

		return fileList.Data;
	}

	public async Task<FileAttachmentGetData?> GetFileAttachmentInfo(List<FileAttachmentGetInfo> files, string fileName)
	{
		Console.WriteLine($"GetFileAttachmentInfo, files count: {files.Count}");
		var filesList = files.Where(f => f.FileName.Equals(fileName)).OrderByDescending(f => f.AttachedTime);
		if (filesList is not null && filesList.Count() > 0)
		{
			var file = filesList.First();
			Console.WriteLine($"GetFileAttachmentInfo, file.FileAttachmentId: {file.FileAttachmentId}");
			var fileData = await _wkycServiceHelper.FileAttachmentGetDataAsync(file.FileAttachmentId);
			return fileData.Data;
		}

		Console.WriteLine($"GetFileAttachmentInfo, files is null");
		return null;
	}

	public async Task<byte[]?> GetFileAttachmentData(List<FileAttachmentGetInfo> files, string fileName)
	{
		var fileGetData = await GetFileAttachmentInfo(files, fileName);
		return fileGetData?.FileData;
	}

	public async Task<byte[]?> GetFileData(List<FileAttachmentGetInfo> files, string fileName)
	{
		var file = files.Where(f => f.FileName.Equals(fileName)).OrderByDescending(f => f.AttachedTime).First();
		if (file is null)
		{
			return null;
		}

		var fileData = await _wkycServiceHelper.FileAttachmentGetDataAsync(file.FileAttachmentId);
		return fileData.Data.FileData;
	}

	public async Task<byte[]?> GetFrontFileData(List<FileAttachmentGetInfo> files)
	{
		return await GetFileData(files, "front_file.jpg");
	}

	public async Task<byte[]?> GetBackFileData(List<FileAttachmentGetInfo> files)
	{
		return await GetFileData(files, "back_file.jpg");
	}

	public async Task<byte[]?> GetSelfieData(List<FileAttachmentGetInfo> files)
	{
		return await GetFileData(files, "selfie.jpg");
	}

	public async Task<FileAttachmentGetData?> GetFrontFileInfo(List<FileAttachmentGetInfo> files)
	{
		return await GetFileAttachmentInfo(files, "front_file.jpg");
	}

	public async Task<FileAttachmentGetData?> GetBackFileInfo(List<FileAttachmentGetInfo> files)
	{
		return await GetFileAttachmentInfo(files, "back_file.jpg");
	}

	public async Task<FileAttachmentGetData?> GetSelfieFileInfo(List<FileAttachmentGetInfo> files)
	{
		return await GetFileAttachmentInfo(files, "selfie.jpg");
	}

	public string ConvertToBase64(byte[] imageData)
	{
		return imageData == null || imageData.Length == 0 ? string.Empty : $"data:image/jpeg;base64,{Convert.ToBase64String(imageData)}";
	}

	public Dictionary<string, string> ReadFileAttachmentDescription(string fileDescription)
	{
		// Create a dictionary to store key-value pairs
		Dictionary<string, string> keyValueDict = new Dictionary<string, string>();
		if (fileDescription.Length > 10)
		{
			// Split the data string by commas to separate key-value pairs
			string[] keyValuePairs = fileDescription.Split(',');
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

	public FileAttachmentAddFileRequest GenerateFileAttachmentAddFileRequest(Guid parentObjectId, int parentObjectTypeId, string fileName, byte[] fileData, string description, bool isViewableByBanker, bool isViewableByCustomer, bool isDeletableByCustomer)
	{
		return new FileAttachmentAddFileRequest()
		{
			ParentObjectId = parentObjectId,
			ParentObjectTypeId = parentObjectTypeId,
			SourceIP = string.Empty,
			FileAttachmentTypeId = 0,
			FileData = fileData,
			FileName = fileName,
			Description = description,
			GroupName = string.Empty,
			ViewableByBanker = isViewableByBanker,
			ViewableByCustomer = isViewableByCustomer,
			DeletableByCustomer = isDeletableByCustomer,
		};
	}

	public async Task<WkycFileAttachemenmtListResponse> GetVLinkDocuments(Guid vlinkId)
	{
		return await _wkycServiceHelper.FileAttachmentGetFileListForObjectAsync(vlinkId.ToString());
	}

	public FileAttachmentAddFileRequest GenerateVLinkFileAttachmentAddFileRequest(Guid vlinkId, string fileName, byte[] fileData, string description)
	{
		return GenerateFileAttachmentAddFileRequest(vlinkId, 28, fileName, fileData, description, true, true, false);
	}

	public async Task<string> ConvertFileToBase64(IBrowserFile file)
	{
		using (var ms = new MemoryStream())
		{
			await file.OpenReadStream(_configuration.GetValue<long>("Win:MaxAllowedSize")).CopyToAsync(ms);
			var bytes = ms.ToArray();
			return Convert.ToBase64String(bytes);
		}
	}

	public async Task<byte[]> ConvertFileToBytes(IBrowserFile file)
	{
		using (var ms = new MemoryStream())
		{
			await file.OpenReadStream(_configuration.GetValue<long>("Win:MaxAllowedSize")).CopyToAsync(ms);
			return ms.ToArray();
		}
	}

	public string ConvertBytesArrayToBase64(byte[] data)
	{
		return data == null || data.Length == 0 ? string.Empty : $"data:image/jpeg;base64,{Convert.ToBase64String(data)}";
	}

	public bool IsImage(string fileName)
	{
		var extension = Path.GetExtension(fileName);
		Console.WriteLine($"IsImage, extension: {extension}");
		return extension.Equals(".png", StringComparison.OrdinalIgnoreCase) ||
			   extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) ||
			   extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase) ||
			   extension.Equals(".gif", StringComparison.OrdinalIgnoreCase);
	}

	public string CreateDownloadLink(byte[] fileBytes)
	{
		using MemoryStream stream = new MemoryStream(fileBytes);
		return $"data:application/octet-stream;base64,{Convert.ToBase64String(stream.ToArray())}";
	}

	public string GenerateSelfieDocumentDescription()
	{
		StringBuilder builder = new StringBuilder();
		builder.Append("documentType: Selfie");
		return builder.ToString();
	}
}
