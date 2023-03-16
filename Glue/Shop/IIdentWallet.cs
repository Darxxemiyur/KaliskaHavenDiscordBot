using KaliskaHaven.Economy;

using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Glue.Shop;

public interface IIdentWallet : IWallet
{
	/// <summary>
	/// Deposit currency to wallet. Always returns the total ammount.
	/// </summary>
	/// <param name="currency"></param>
	/// <returns></returns>
	new Task<IIdentifiable<ITransactionLog>> Deposit(Currency currency);

	/// <summary>
	/// </summary>
	/// <param name="currency"></param>
	/// <returns></returns>
	new Task<IIdentifiable<ITransactionLog>> Withdraw(Currency currency);
}