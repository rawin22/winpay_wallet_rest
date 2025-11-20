namespace Wallet.Interfaces;

public interface IQRCodeReaderService
{
	string ReadQRCode(string imagePath);
	string DecodeQRCode(string base64Image);
}