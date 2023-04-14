using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Shop;

/// <summary>
/// Target that acquires afforded items.
/// </summary>
public interface ICustomer
{
	/// <summary>
	/// Apply post result with removal of reservations.
	/// </summary>
	/// <param name="postResult"></param>
	/// <returns></returns>
	Task ApplyPostResult(IPostResult postResult);
	/// <summary>
	/// Make Post Result Reservation
	/// </summary>
	/// <param name="postResult"></param>
	/// <returns></returns>
	Task ApplyPRReservation(IPostResult postResult);
	/// <summary>
	/// Revert Post Result reservation
	/// </summary>
	/// <param name="postResult"></param>
	/// <returns></returns>
	Task RevertPRReservation(IPostResult postResult);

	/// <summary>
	/// Accept requirement for judging.
	/// </summary>
	/// <param name="requirement">Requirement</param>
	/// <returns>True if requirement is satisfied. False otherwise.</returns>
	Task<bool> AcceptRequirement(IRequirement requirement, ICartItem details);

	/// <summary>
	/// Presents an ambiguous and un-typed way of communication with the implementation.
	/// </summary>
	/// <returns></returns>
	Task<ICustomerCommunicable> GetCommunicator();
}
