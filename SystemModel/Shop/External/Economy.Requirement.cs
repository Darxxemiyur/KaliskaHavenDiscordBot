using KaliskaHaven.Economy;

using IWallet = KaliskaHaven.Shop.External.IIdentWallet;

namespace KaliskaHaven.Shop.External;

public class EconomyRequirement : IRequirement
{
	public string RequirementType => nameof(EconomyRequirement);
	private static readonly Type[] RTypes = new Type[] { typeof(IWallet) };
	public IEnumerable<Type> RequiredTypes => RTypes;
	public Currency Price => Data.Money;

	private IEconomyData Data {
		get;
	}

	public EconomyRequirement(IEconomyData data) => Data = data;

	public async Task<bool> CustomerVisit(ICustomer customer)
	{
		var commie = await customer.GetCommunicator();

		var res = await commie.TellInternalAsync(new EcoTellMessage(EcoTellMsgEnum.GetWallet));

		if (res.Code != 0 || res.Result is not IWallet wallet)
			return false;

		var currency = await wallet.Get(Price.CurrencyType);

		return Price.Quantity <= (currency?.Quantity ?? 0);
	}
}