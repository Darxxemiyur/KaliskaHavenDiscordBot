using KaliskaHaven.Shop;

using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Database.Shop;

public sealed class ShopItem : IShopItem, IIdentifiable<IShopItem>
{
	public ulong ID {
		get; set;
	}

	public IEnumerable<IRequirement> PreRequestiments {
		get; set;
	}

	public IEnumerable<IPostResult> PostResults {
		get; set;
	}

	public IIdentity? Identity => throw new NotImplementedException();
	public IShopItem? Identifyable => this;

	public Type Type {
		get;
	} = typeof(ShopItem);

	public bool Equals<TId>(IIdentifiable<TId> to) => to is ShopItem si && si.ID == ID;
}
