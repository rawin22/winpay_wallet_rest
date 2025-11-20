using GPWebApi.DTO;

namespace Wallet.Models;

public class WkycCountryListResponse : WkycBaseResponse
{
    public List<CountryData> Countries { get; set; }
}
