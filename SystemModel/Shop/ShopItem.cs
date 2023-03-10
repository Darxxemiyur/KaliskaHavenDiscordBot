using KaliskaHaven.Shop;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
	public interface IShopItem
	{
		LinkedList<IPreRequestiment> PreRequestiments {
			get;
		}
		LinkedList<IPostResult> PostResults {
			get;
		}
	}
}
