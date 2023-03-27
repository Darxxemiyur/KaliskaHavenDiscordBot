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
	/// The item. Because an Item is never guaranteed to have strict set of properties, the best way
	/// to access it would be soft.
	/// </summary>
	JObject Item {
		get;
	}
}
