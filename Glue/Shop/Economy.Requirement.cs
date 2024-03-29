﻿using KaliskaHaven.Database.Economy;
using KaliskaHaven.Economy;
using KaliskaHaven.Shop;

using Name.Bayfaderix.Darxxemiyur.General;

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

	public IIdentity? Identity {
		get;
	}

	public IRequirement? Identifyable {
		get;
	}

	public EconomyRequirement(IEconomyData data) => Data = data;

	///<inheritdoc/>
	public async Task<bool> CustomerVisit(ICustomer customer)
	{
		if (await customer.GetWallet() is var wallet && wallet is null)
			return false;

		var currency = await wallet.Get(Price.CurrencyType);

		return Price.Quantity <= (currency?.Quantity ?? 0);
	}

	public bool Equals<TId>(IIdentifiable<TId> to) => throw new NotImplementedException();
}
