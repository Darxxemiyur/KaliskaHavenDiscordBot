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

	ICollection<ICartItem> Cart {
		get;
	}

	ICollection<IRequirement> Requirements {
		get;
	}

	ICollection<IPostResult> PostResults {
		get;
	}

	Task<ICartItem> AddToCard(IShopItem item);

	Task<bool> RemoveFromCart(ICartItem item);
}
