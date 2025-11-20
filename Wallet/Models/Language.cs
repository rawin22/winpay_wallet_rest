using System.Text.Json.Serialization;

namespace Wallet.Models
{
	public class Language
	{
		public int Id { get; set; } = 0;
		public string Locale { get; set; } = "";

		[JsonPropertyName("name_en")]
		public string NameEn { get; set; } = "";
		public string Name { get; set; } = "";
		public string FlagCode => Locale.Equals("en_us", StringComparison.OrdinalIgnoreCase) ? "us" : Locale.Split('_')[1].ToLower();
	};
}
