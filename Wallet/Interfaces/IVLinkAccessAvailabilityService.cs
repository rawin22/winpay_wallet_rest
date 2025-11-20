namespace Wallet.Interfaces;

public enum AccessAvailability
{
	Public = 0,
	Member = 1,
	[StringValue("Verified Member")]
	VerifiedMember = 2
}

[AttributeUsage(AttributeTargets.Field)]
public class StringValueAttribute : Attribute
{
	public string Value { get; }

	public StringValueAttribute(string value)
	{
		Value = value;
	}
}

public static class EnumExtensions
{
	public static string GetStringValue(this Enum value)
	{
		var fieldInfo = value.GetType().GetField(value.ToString());
		var attribute = (StringValueAttribute)fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false)[0];
		return attribute.Value;
	}
}

internal interface IVLinkAccessAvailabilityService
{
	AccessAvailability GetAccessAvailability(int number);
	string GetAccessAvailable(int number);
}