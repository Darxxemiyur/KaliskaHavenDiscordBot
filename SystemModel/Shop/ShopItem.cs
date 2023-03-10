using KaliskaHaven.Shop;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
	public sealed class ShopItem
	{
		public LinkedList<IRequirement> PreRequestiments {
			get; set;
		}
		public LinkedList<IPostResult> PostResults {
			get; set;
		}
	}
}
