
using GPWebApi.DTO;

namespace Wallet.Models
{
	public class WkycCustomerUserCreateResponse

	{
		public bool IsSuccessful { get; set; } = false;
		public List<string> ErrorMessages { get; set; } = new List<string>();
		public CustomerUserCreateData User { get; set; }
	}
}
