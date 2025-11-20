using GPWebApi.DTO;

namespace Wallet.Models
{
	public class WkycGetCustomerResponse

	{
		public bool IsSuccessful { get; set; } = false;
		public List<string> ErrorMessages { get; set; } = new List<string>();
		public CustomerGetData Data { get; set; } = new CustomerGetData();
	}
}
