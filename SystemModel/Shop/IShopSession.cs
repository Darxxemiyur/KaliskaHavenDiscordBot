using KaliskaHaven.Economy;

using Shop;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaliskaHaven.Shop
{
	public interface IShopSession
	{
		public Task<IWallet> CustomerWallet {
			get;
		}
		public IAsyncEnumerable<ICartItem> Cart {
			get;
		}
		public Task<ICartItem> AddToCard(ShopItem item);
	}
	public interface ICartItem
	{
		ShopItem ShopItem {
			get;
		}
		Task Remove();
	}
}
