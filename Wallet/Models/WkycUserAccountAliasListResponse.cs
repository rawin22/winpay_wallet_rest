using GPWebApi.DTO;

namespace Wallet.Models;

public class WkycUserAccountAliasListResponse : WkycBaseResponse
{
	public List<UserAccountAliasData> AccountAliases { get; set; }
}
