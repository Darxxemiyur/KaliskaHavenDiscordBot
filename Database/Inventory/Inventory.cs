using KaliskaHaven.Inventory;

using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Database.Inventory
{
	public sealed class Inventory : IInventory
	{
		public IAsyncEnumerable<IItem> Items {
			get;
		}

		IAsyncEnumerable<IIdentifiable<IItem>> IInventory.Items {
			get;
		}

		public Task InsertItem(IItem item) => throw new NotImplementedException();

		public Task<(bool status, IIdentifiable<IItem> item)> InsertItem(IIdentifiable<IItem> item) => throw new NotImplementedException();

		public Task RemoveItem(IItem item) => throw new NotImplementedException();

		public Task<bool> RemoveItem(IIdentifiable<IItem> item) => throw new NotImplementedException();

		public Task UpdateItem(IItem item) => throw new NotImplementedException();

		public Task<bool> UpdateItem(IIdentifiable<IItem> item) => throw new NotImplementedException();

		public Task<bool> UpdateItem(IIdentifiable<IItem> item, Func<IIdentifiable<IItem>, Task> changer) => throw new NotImplementedException();

		Task<(bool status, IIdentifiable<IItem> item)> IInventory.InsertItem(IItem item) => throw new NotImplementedException();
	}
}
