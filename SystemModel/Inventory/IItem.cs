using Newtonsoft.Json.Linq;

namespace KaliskaHaven.Inventory;

public interface IItem
{
	/// <summary>
	/// Ascquire Item Type
	/// </summary>
	Task<Type?> ItemType {
		get;
	}

	/// <summary>
	/// Optional item tags.
	/// </summary>
	ICollection<string> Tags {
		get;
	}

	/// <summary>
	/// The item.
	/// </summary>
	JObject Item {
		get;
	}
}
