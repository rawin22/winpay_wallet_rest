
using GPWebApi.DTO;

namespace Wallet.Models
{
	public class WkycCustomerCreateFromTemplateResponse

	{
		public bool IsSuccessful { get; set; } = false;
		public List<string> ErrorMessages { get; set; } = new List<string>();
		public CustomerCreateFromTemplateData Customer { get; set; }
	}
}
