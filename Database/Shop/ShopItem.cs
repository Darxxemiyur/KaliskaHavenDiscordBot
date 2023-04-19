using KaliskaHaven.Shop;

using Name.Bayfaderix.Darxxemiyur.Extensions;
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

	public ICollection<IRequirement>? InnerPreRequestiments {
		get;
	}

	public ICollection<IPostResult>? InnerPostResults {
		get;
	}

	public ICollection<IOptionInfo>? InnerOptions {
		get;
	}

	public ICollection<IRequirement> PreRequestiments => InnerPreRequestiments.ThrowIfNull();

	public ICollection<IPostResult> PostResults => InnerPostResults.ThrowIfNull();

	public ICollection<IOptionInfo> Options => InnerOptions.ThrowIfNull();

	public bool Equals<TId>(IIdentifiable<TId> to) => to is ShopItem si && si.ID == ID;
}
