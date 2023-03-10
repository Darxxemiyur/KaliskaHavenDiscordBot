using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KaliskaHaven.Inventory
{
	public sealed class InventoryItem
	{
		public string ItemType => ItemKindU?.RootElement.GetProperty(nameof(ItemType)).GetString() ?? throw new ArgumentNullException(nameof(ItemKindU),$"{nameof(ItemKindU)} was null!");
		public JsonDocument? ItemKindU {
			get; set;
		}
		public InventoryItem()
		{

		}
	}
}
