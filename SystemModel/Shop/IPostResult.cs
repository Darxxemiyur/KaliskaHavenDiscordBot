namespace KaliskaHaven.Shop;

public interface IPostResult
{
	/// <summary>
	/// Apply reservations.
	/// </summary>
	/// <param name="customer"></param>
	/// <param name="details"></param>
	Task ApplyOnCustomer(ICustomer customer, ICartItem details);

	/// <summary>
	/// Do required reservation to apply later.
	/// </summary>
	/// <param name="customer"></param>
	/// <param name="details"></param>
	/// <returns>True if reservation is prepared. False otherwise.</returns>
	Task<bool> TryReserveOnCustomer(ICustomer customer, ICartItem details);

	/// <summary>
	/// Revert applied/reserved rervations.
	/// </summary>
	/// <param name="customer"></param>
	/// <returns></returns>
	Task Revert(ICustomer customer);
}