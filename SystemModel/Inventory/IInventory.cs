using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaliskaHaven.Inventory
{
	public interface IInventory
	{
		IAsyncEnumerable<InventoryItem> Items {
			get;
		}
		Task RemoveItem(InventoryItem item);
		Task InsertItem(InventoryItem item);
		Task UpdateItem(InventoryItem item);
	}
}
