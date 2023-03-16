namespace KaliskaHaven.Shop;

public interface IShopItem
{
	ICollection<IRequirement> PreRequestiments {
		get;
	}

	ICollection<IPostResult> PostResults {
		get;
	}

	ICollection<IOptionInfo> Options {
		get;
	}
}
