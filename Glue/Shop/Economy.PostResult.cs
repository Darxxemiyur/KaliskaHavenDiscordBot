using KaliskaHaven.Economy;
using KaliskaHaven.Shop;

namespace KaliskaHaven.Glue.Shop;

public abstract class EconomyPostResult : IPostResult
{
	public Currency Price => Data.Money;

	private IEconomyData Data {
		get;
	}

	protected EconomyPostResult(IEconomyData data) => Data = data;

	public async Task Revert(ICustomer customer)
	{
	}

	public async Task ApplyOnCustomer(ICustomer customer, ICartItem details)
	{
		var commie = await customer.GetCommunicator();
		if (await GetWallet(commie) is var wallet && wallet is null)
			throw new InvalidOperationException();

		await this.WithdrawAndLog(customer, wallet, details, this.CalculatePrice(details));
	}

	protected abstract Task WithdrawAndLog(ICustomer customer, IIdentWallet wallet, ICartItem details, Currency currency);

	protected Currency CalculatePrice(ICartItem citem) => new() {
		CurrencyType = Price.CurrencyType,
		Quantity = (long)Math.Round(Price.Quantity * citem.Quantity)
	};

	private static async Task<IIdentWallet?> GetWallet(CustomerCommunicable commie)
	{
		var res = await commie.TellInternalAsync(new EcoTellMessage(EcoTellMsgEnum.GetWallet));

		return res.Code != 0 || res.Result is not IIdentWallet wallet ? null : wallet;
	}

	public async Task<bool> TryReserveOnCustomer(ICustomer customer, ICartItem details)
	{
		var commie = await customer.GetCommunicator();
		if (await GetWallet(commie) is var wallet && wallet is null)
			return false;

		var currency = await wallet.Get(Price.CurrencyType);
		var price = this.CalculatePrice(details);

		return price.Quantity <= (currency?.Quantity ?? 0);
	}
}