using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Economy;

public interface IWallet
{
	/// <summary>
	/// Deposit currency to wallet. Always returns the total ammount.
	/// </summary>
	/// <param name="currency"></param>
	/// <returns></returns>
	Task<IIdentifiable<ITransactionLog>> Deposit(Currency currency);

	/// <summary>
	/// </summary>
	/// <param name="currency"></param>
	/// <returns></returns>
	Task<IIdentifiable<ITransactionLog>> Withdraw(Currency currency);

	/// <summary>
	/// Get specific Wallet's currency.
	/// </summary>
	/// <param name="currency">Type of currency to get</param>
	/// <returns>The currency of the type if any</returns>
	Task<Currency?> Get(CurrencyType currency);

	/// <summary>
	/// Get Wallet's currencies.
	/// </summary>
	/// <returns></returns>
	IAsyncEnumerable<Currency> GetAll();
}