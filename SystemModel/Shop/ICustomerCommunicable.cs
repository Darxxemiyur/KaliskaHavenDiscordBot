using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Shop
{
	/// <summary>
	/// To allow making extensions from ICustomerCommunicable, and keep IMessageCommunicable clean.
	/// </summary>
	public interface ICustomerCommunicable : IMessageCommunicable
	{
	}
}
