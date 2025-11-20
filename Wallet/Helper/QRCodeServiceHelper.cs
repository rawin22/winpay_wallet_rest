using QRCoder;
using SkiaSharp;
using System.Runtime.Versioning;

namespace Wallet.Helper
{
	public static class QRCodeServiceHelper
	{
		/*		private static readonly string[] LogoPaths =
				{
					"images/qr-code-icons/winstantpay.png",
					"images/qr-code-icons/kyc.png"
				};
		*/
		public static SKBitmap GenerateAsSKBitmap(string text, string logoFile)
		{
			using var qrGenerator = new QRCodeGenerator();
			var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
			var qrCodeSize = qrCodeData.ModuleMatrix.Count;

			// Calculate scale to fit the QR code in the 400x400 bitmap
			float scale = 400f / qrCodeSize;
			var bitmap = new SKBitmap(400, 400);

			using (var canvas = new SKCanvas(bitmap))
			{
				canvas.Clear(SKColors.White);
				var paint = new SKPaint
				{
					Color = SKColors.Black,
					IsAntialias = true,
				};

				for (int y = 0; y < qrCodeSize; y++)
				{
					for (int x = 0; x < qrCodeSize; x++)
					{
						if (qrCodeData.ModuleMatrix[y][x])
						{
							// Draw each module scaled to match the 400x400 size
							canvas.DrawRect(x * scale, y * scale, scale, scale, paint);
						}
					}
				}


				using var logo = SKBitmap.Decode(logoFile);
				if (logo != null)
				{
					// Calculate logo size and position
					int logoSize = Math.Min(400, 400) / 5; // Example: logo size to be 1/5th of the QR code size
					var logoRect = new SKRect(400 / 2f - logoSize / 2f, 400 / 2f - logoSize / 2f, 400 / 2f + logoSize / 2f, 400 / 2f + logoSize / 2f);
					canvas.DrawBitmap(logo, logoRect);
				}
			}

			return bitmap;
		}

		[SupportedOSPlatform("windows6.1")]
		public static string GenerateAsBase64(string text, string logoFile)
		{
			using var bitmap = GenerateAsSKBitmap(text, logoFile);
			using var image = SKImage.FromBitmap(bitmap);
			using var data = image.Encode(SKEncodedImageFormat.Png, 100);
			return Convert.ToBase64String(data.ToArray());
		}

		public static void GenerateAndSaveBitmapToFile(string text, string logoFile)
		{
			using var bitmap = GenerateAsSKBitmap(text, logoFile);
			using var image = SKImage.FromBitmap(bitmap);
			using var data = image.Encode(SKEncodedImageFormat.Png, 100);
			File.WriteAllBytes("test_qr_code.png", data.ToArray());
		}
	}
}
