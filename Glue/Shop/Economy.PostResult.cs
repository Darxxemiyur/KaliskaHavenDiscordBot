using KaliskaHaven.Database.Economy;
using KaliskaHaven.Economy;
using KaliskaHaven.Shop;

using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Glue.Shop;

public abstract class EconomyPostResult : IPostResult
{
	public Currency Price => Data.Money;

	private IEconomyData Data {
		get;
	}
	public abstract IIdentity? Identity {
		get;
	}
	public abstract IPostResult? Identifyable {
		get;
	}

	protected EconomyPostResult(IEconomyData data) => Data = data;

	public async Task Revert(ICustomer customer)
	{
	}

	public async Task ApplyOnCustomer(ICustomer customer, ICartItem details)
	{
		if (await customer.GetWallet() is var wallet && wallet is null)
			throw new InvalidOperationException();

		await this.WithdrawAndLog(customer, wallet, details, this.CalculatePrice(details));
	}

	protected abstract Task WithdrawAndLog(ICustomer customer, IDbWallet wallet, ICartItem details, Currency currency);

	protected Currency CalculatePrice(ICartItem citem) => new(Price.CurrencyType, (long)Math.Round(Price.Quantity / 1f));

	public async Task<bool> TryReserveOnCustomer(ICustomer customer, ICartItem details)
	{
		if (await customer.GetWallet() is var wallet && wallet is null)
			return false;

		var currency = await wallet.Get(Price.CurrencyType);
		var price = this.CalculatePrice(details);

		return price.Quantity <= (currency?.Quantity ?? 0);
	}

	public abstract bool Equals<TId>(IIdentifiable<TId> to);
}
