using GPWebApi.DTO;

namespace Wallet.Models;

public class WkycCountryIdentificationTypeListResponse : WkycBaseResponse
{
	public List<CountryIdentificationTypeData> IdentificationTypes { get; set; }
}
