using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Shop;

public interface IShopItem : IIdentifiable<IShopItem>
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
