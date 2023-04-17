using KaliskaHaven.Database.Economy;
using KaliskaHaven.Shop;

namespace KaliskaHaven.Glue.Shop;

public static class ShopExtensions
{
	public static async Task<IDbWallet?> GetWallet(this ICustomer customer)
	{
		var commie = await customer.GetCommunicator();

		return await commie.GetWallet();
	}

	public static async Task<IDbWallet?> GetWallet(this ICustomerCommunicable customerC)
	{
		var res = await customerC.TellInternalAsync(new EcoTellMessage(EcoTellMsgEnum.GetWallet));

		return res.Code != 0 || res.Result is not IDbWallet wallet ? null : wallet;
	}
}
