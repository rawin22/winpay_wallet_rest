using GPWebApi.DTO;

namespace Wallet.Models;

public class WkycCustomerAccountAliasListResponse : WkycBaseResponse
{
	public List<CustomerAccountAliasData> Aliases { get; set; }
}
