namespace KaliskaHaven.Shop;

public interface ICartItem
{
	IShopSession Session {
		get;
	}

	IShopItem ShopItem {
		get;
	}

	ICollection<IConfigurableOption> Options {
		get;
	}

	decimal Quantity {
		get;
	}

	Task Remove();
}
