
using GPWebApi.DTO;

namespace Wallet.Models
{
	public class WkycCustomerCreateResponse

	{
		public bool IsSuccessful { get; set; } = false;
		public List<string> ErrorMessages { get; set; } = new List<string>();
		public CustomerCreateData Customer { get; set; }
	}
}
