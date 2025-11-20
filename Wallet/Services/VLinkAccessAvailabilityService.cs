using Wallet.Interfaces;

namespace Wallet.Services;

public class VLinkAccessAvailabilityService : IVLinkAccessAvailabilityService
{
	private readonly string[] AccessAvailableArray = ["Public", "Members", "Verified Members"];

	public AccessAvailability GetAccessAvailability(int number)
	{
		// Implement logic to return the corresponding enum based on the number
		// You can use switch-case, LINQ, or any other method to map the number to the enum
		switch (number)
		{
			case 0:
				return AccessAvailability.Public;
			case 1:
				return AccessAvailability.Member;
			case 2:
				return AccessAvailability.VerifiedMember;
			default:
				return AccessAvailability.Public;
		}
	}

	//public string GetAccessAvailable(int number)
	//{
	//	// Implement logic to return the corresponding enum based on the number
	//	// You can use switch-case, LINQ, or any other method to map the number to the enum
	//	AccessAvailability access;
	//	switch (number)
	//	{
	//		case 0:
	//			//return AccessAvailability.Public;
	//			access = AccessAvailability.Public;
	//			return access.GetStringValue();
	//		case 1:
	//			//return AccessAvailability.Member;

	//			access = AccessAvailability.Member;
	//			return access.GetStringValue();
	//		case 2:
	//			access = AccessAvailability.VerifiedMember;
	//			return access.GetStringValue();
	//		default:
	//			//return AccessAvailability.Public;

	//			access = AccessAvailability.Public;
	//			return access.GetStringValue();
	//	}
	//}

	public string GetAccessAvailable(int number)
	{
		return AccessAvailableArray[number];
	}
}
