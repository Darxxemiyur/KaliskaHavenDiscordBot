using KaliskaHaven.Shop;

using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Database.Shop;

public sealed class ShopItem : IShopItem
{
	public ulong ID {
		get; set;
	}

	public IIdentity? Identity => throw new NotImplementedException();
	public IShopItem? Identifyable => this;

	public Type Type {
		get;
	} = typeof(ShopItem);

	public IAcquirable<ICollection<IRequirement>> PreRequestiments {
		get;
	}

	public IAcquirable<ICollection<IPostResult>> PostResults {
		get;
	}

	public IAcquirable<ICollection<IOptionInfo>> Options {
		get;
	}

	public bool Equals<TId>(IIdentifiable<TId> to) => to is ShopItem si && si.ID == ID;
}
