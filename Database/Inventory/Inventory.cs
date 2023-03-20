using KaliskaHaven.Inventory;

namespace KaliskaHaven.Database.Inventory
{
	public sealed class Inventory : IInventory
	{
		public IAsyncEnumerable<IItem> Items {
			get;
		}

		public Task InsertItem(IItem item) => throw new NotImplementedException();
		public Task RemoveItem(IItem item) => throw new NotImplementedException();
		public Task UpdateItem(IItem item) => throw new NotImplementedException();
	}
}
