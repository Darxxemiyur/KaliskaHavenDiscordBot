using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Shop;

public interface IShopItem : IIdentifiable<IShopItem>
{
	IAcquirable<ICollection<IRequirement>> PreRequestiments {
		get;
	}

	IAcquirable<ICollection<IPostResult>> PostResults {
		get;
	}

	IAcquirable<ICollection<IOptionInfo>> Options {
		get;
	}
}
