using KaliskaHaven.Inventory;

using Newtonsoft.Json.Linq;

namespace KaliskaHaven.Database.Inventory
{
	public sealed class InventoryItem : IItem
	{
		public Task<Type?> ItemType => this.GetItemType();

		public ICollection<string> Tags {
			get; set;
		}

		public JObject Item {
			get; set;
		}

		private async Task<Type?> GetItemType()
		{
			var pn = nameof(ItemType);
			var re = Item;

			if (!re.TryGetValue(pn, out var typeP) || typeP.Type != JTokenType.String)
				return null;

			var typeName = typeP.ToObject<string>();
			if (typeName == null)
				return null;

			return await Task.Run(() => Type.GetType(typeName));
		}

		public void SetItemType(Type type)
		{
			var pn = nameof(ItemType);
			var re = Item;

			var rep = JToken.FromObject(type.AssemblyQualifiedName);
			if (re.TryGetValue(pn, out var typeP))
				typeP.Replace(rep);
			else
				re.Add(pn, rep);
		}
	}
}
