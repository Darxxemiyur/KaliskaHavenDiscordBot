namespace KaliskaHaven.Inventory;

public interface IInventory
{
	IAsyncEnumerable<IItem> Items {
		get;
	}

	Task RemoveItem(IItem item);

	Task InsertItem(IItem item);

	Task UpdateItem(IItem item);
}
