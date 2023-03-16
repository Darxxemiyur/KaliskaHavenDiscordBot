namespace KaliskaHaven.Inventory;

public interface IInventory
{
	IAsyncEnumerable<InventoryItem> Items {
		get;
	}

	Task RemoveItem(InventoryItem item);

	Task InsertItem(InventoryItem item);

	Task UpdateItem(InventoryItem item);
}