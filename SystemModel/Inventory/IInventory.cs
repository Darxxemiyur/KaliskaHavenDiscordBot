using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Inventory;

/// <summary>
/// An interface of how an inventory should be done.
/// </summary>
public interface IInventory
{
	/// <summary>
	/// An async enumerable that yields the items of this inventory.
	/// </summary>
	IAsyncEnumerable<IIdentifiable<IItem>> Items {
		get;
	}

	/// <summary>
	/// Remove inventory item.
	/// </summary>
	/// <param name="item"></param>
	/// <returns>True if it was removed. False otherwise.</returns>
	Task<bool> RemoveItem(IIdentifiable<IItem> item);

	/// <summary>
	/// Insert an item into inventory.
	/// </summary>
	/// <param name="item">An item to insert.</param>
	/// <returns>
	/// A tuple with status of operation, and the identifiable inserted item, if it was inserted.
	/// </returns>
	Task<(bool status, IIdentifiable<IItem> item)> InsertItem(IItem item);

	/// <summary>
	/// Insert an identifiable item into inventory.
	/// </summary>
	/// <param name="item">An identifiable item to insert.</param>
	/// <returns>
	/// A tuple with status of operation and the identifiable inserted item, if it was inserted.
	/// </returns>
	Task<(bool status, IIdentifiable<IItem> item)> InsertItem(IIdentifiable<IItem> item);

	/// <summary>
	/// Updates item
	/// </summary>
	/// <param name="item"></param>
	/// <returns>True if it was saved. False otherwise.</returns>
	Task<bool> UpdateItem(IIdentifiable<IItem> item);

	/// <summary>
	/// Updates item with delegate to later apply on that item.
	/// </summary>
	/// <param name="item">The item with identity.</param>
	/// <param name="changer">The item changing delegate.</param>
	/// <returns>True if it was saved. False otherwise.</returns>
	Task<bool> UpdateItem(IIdentifiable<IItem> item, Func<IIdentifiable<IItem>, Task> changer);
}
