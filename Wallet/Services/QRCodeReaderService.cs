using SkiaSharp;
using Wallet.Interfaces;
using ZXing.SkiaSharp;

namespace Wallet.Services;

public class QRCodeReaderService : IQRCodeReaderService
{
    public string ReadQRCode(string imagePath)
    {
        var barcodeReader = new BarcodeReader();
        using (var bitmap = SKBitmap.Decode(imagePath))
        {
            if (bitmap == null)
            {
                return "Failed to load image.";
            }
            var result = barcodeReader.Decode(bitmap);
            return result?.Text ?? "QR code not found.";
        }
    }

    public string DecodeQRCode(string base64Image)
    {
        var barcodeReader = new BarcodeReader();
        var imageBytes = Convert.FromBase64String(base64Image);

        using (var stream = new MemoryStream(imageBytes))
        {
            using (var bitmap = SKBitmap.Decode(stream))
            {
                if (bitmap == null)
                {
                    return "Failed to load image.";
                }
                var result = barcodeReader.Decode(bitmap);
                return result?.Text ?? "QR code not found.";
            }
        }
    }
}