using KaliskaHaven.Economy;
using KaliskaHaven.Shop.External;

using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Database.Shop;

public sealed class EconomyReqResData : IEconomyData, IIdentifiable<IEconomyData>
{
	public IIdentity? Identity => throw new NotImplementedException();
	public ulong ID {
		get; set;
	}
	public Currency Money {
		get;
		set;
	}

	public IEconomyData? Identifyable => this;

	public Type Type {
		get;
	} = typeof(EconomyReqResData);
	public bool Equals<TId>(IIdentifiable<TId> to) => to is EconomyReqResData si && si.ID == ID;
}
