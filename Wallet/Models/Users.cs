using System.ComponentModel.DataAnnotations;

namespace Wallet.Models;

public class UserPasswordChangeViewModel
{
	public Guid UserId { get; set; }
	public string OldPassword { get; set; } = string.Empty;
	public string NewPassword { get; set; }
	public string ConfirmNewPassword { get; set; }
}

public class UserSignUpModel
{
	[Required]
	public string UserName { get; set; }

	[Required]
	[DataType(DataType.Password)]
	public string Password { get; set; }

	[Required]
	[DataType(DataType.Password)]
	public string ConfirmPassword { get; set; }

	[Required]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	public string FirstName { get; set; }

	[Required]
	public string LastName { get; set; }

	[Required]
	public string ReferredBy { get; set; }
}