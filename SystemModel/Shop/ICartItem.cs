using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Shop;

/// <summary>
/// Cart item. Holds the shop item, session's "cart" this cart item belongs to, and configurable options.
/// </summary>
public interface ICartItem : IIdentifiable<ICartItem>
{
	/// <summary>
	/// Shop session this cart item belongs to.
	/// </summary>
	IShopSession Session {
		get;
	}

	/// <summary>
	/// The original shop item.
	/// </summary>
	IShopItem ShopItem {
		get;
	}

	/// <summary>
	/// Set of options available for this cart item.
	/// </summary>
	ICollection<IConfigurableOption> Options {
		get;
	}

	/// <summary>
	/// Remove item from session's cart.
	/// </summary>
	/// <returns></returns>
	Task Remove();
}
