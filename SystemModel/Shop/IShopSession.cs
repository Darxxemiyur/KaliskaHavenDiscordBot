using KaliskaHaven.Economy;

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

	}
}
