using KaliskaHaven.Database.Economy;
using KaliskaHaven.Database.Shop;

using Microsoft.EntityFrameworkCore;

namespace KaliskaHaven.Database
{
	public class KaliskaDB : DbContext
	{
		public DbSet<ShopItem> ShopItems {
			get; set;
		}

		public DbSet<DbCurrency> DbCurrencies {
			get; set;
		}

		public DbSet<TransactionRecord> TransactionRecords {
			get; set;
		}

		public DbSet<Wallet> Wallets {
			get; set;
		}
	}
}
