using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Shop;

public interface IShopItem
{
	IMetaIdentity MetaIdentity {
		get;
	}
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
