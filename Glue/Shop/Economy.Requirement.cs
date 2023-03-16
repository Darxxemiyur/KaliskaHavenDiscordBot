using KaliskaHaven.Database.Economy;
using KaliskaHaven.Economy;
using KaliskaHaven.Shop;

namespace KaliskaHaven.Glue.Shop;

public class EconomyRequirement : IRequirement
{
	public string RequirementType => nameof(EconomyRequirement);
	private static readonly Type[] RTypes = new Type[] { typeof(IDbWallet) };
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

		if (res.Code != 0 || res.Result is not IDbWallet wallet)
			return false;

		var currency = await wallet.Get(Price.CurrencyType);

		return Price.Quantity <= (currency?.Quantity ?? 0);
	}
}
