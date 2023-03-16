using KaliskaHaven.Economy;

using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Shop.External;

public interface IIdentWallet : IWallet
{
	/// <summary>
	/// Deposit currency to wallet. Always returns the total ammount.
	/// </summary>
	/// <param name="currency"></param>
	/// <returns></returns>
	new Task<IIdentifiable<TransactionLog>> Deposit(Currency currency);

	/// <summary>
	/// </summary>
	/// <param name="currency"></param>
	/// <returns></returns>
	new Task<IIdentifiable<TransactionLog>> Withdraw(Currency currency);
}