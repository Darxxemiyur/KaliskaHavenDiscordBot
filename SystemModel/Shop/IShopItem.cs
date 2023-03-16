namespace KaliskaHaven.Shop;

public interface IShopItem
{
	IEnumerable<IRequirement> PreRequestiments {
		get;
	}

	IEnumerable<IPostResult> PostResults {
		get;
	}
}