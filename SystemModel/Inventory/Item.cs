using System.Text.Json;

namespace KaliskaHaven.Inventory;

public sealed class InventoryItem
{
	public string ItemType => ItemKindU?.RootElement.GetProperty(nameof(ItemType)).GetString() ?? throw new ArgumentNullException(nameof(ItemKindU), $"{nameof(ItemKindU)} was null!");

	public JsonDocument? ItemKindU {
		get; set;
	}

	public InventoryItem()
	{
	}
}