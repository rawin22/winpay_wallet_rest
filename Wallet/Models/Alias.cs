namespace Wallet.Models
{
	public class NewCustomerAccountAlias
	{
		public Guid CustomerId { get; set; }
		public string AccountAlias { get; set; }
		public int AccountAliasType { get; set; } = 2;
		public bool SetAsDefault { get; set; } = false;
	}
}
