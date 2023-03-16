using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Shop;

public interface IShopSession
{
	/// <summary>
	/// Way to communicate with shop implementation.
	/// </summary>
	IMessageCommunicable Communicable {
		get;
	}

	IAsyncEnumerable<ICartItem> Cart {
		get;
	}

	IAsyncEnumerable<IRequirement> Requirements {
		get;
	}

	IAsyncEnumerable<IPostResult> PostResults {
		get;
	}

	Task<ICartItem> AddToCard(IShopItem item);

	Task<bool> RemoveFromCart(ICartItem item);
}

public interface ICartItem
{
	IShopSession Session {
		get;
	}

	IShopItem ShopItem {
		get;
	}

	decimal Quantity {
		get;
	}

	Task Remove();
}