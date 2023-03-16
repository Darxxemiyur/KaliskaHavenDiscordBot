namespace KaliskaHaven.Shop;

public interface IShopItem
{
	LinkedList<IRequirement> PreRequestiments {
		get;
	}

	LinkedList<IPostResult> PostResults {
		get;
	}
}