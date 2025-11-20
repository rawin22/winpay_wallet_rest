namespace Wallet.Models;

public class CountryIdentificationType
{
	public int Id { get; set; } = 0;
	public string CountryCode { get; set; } = string.Empty;
	public string IDTypeName { get; set; } = string.Empty;
	public string IDTypeEnglishName { get; set; } = string.Empty;
	public string IDCategory { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public bool HasFullSpread { get; set; } = false;
	public bool HasBioDataPage { get; set; } = false;
	public bool RequireFrontSide { get; set; } = false;
	public bool RequireBackSide { get; set; } = false;
	public string? Notes { get; set; } = string.Empty;
	public int SortOrder { get; set; } = 0;
}
