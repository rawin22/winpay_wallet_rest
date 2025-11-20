using GPWebApi.DTO;

namespace Wallet.Models;

public class WkycPaymentCurrencyListResponse : WkycBaseResponse
{
	public List<CurrencyData> Currencies { get; set; }
}
