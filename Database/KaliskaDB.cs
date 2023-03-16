using KaliskaHaven.Database.Shop;

using Microsoft.EntityFrameworkCore;

namespace KaliskaHaven.Database
{
	public class KaliskaDB : DbContext
	{
		public DbSet<ShopItem> ShopItems {
			get; set;
		}

	}
}
