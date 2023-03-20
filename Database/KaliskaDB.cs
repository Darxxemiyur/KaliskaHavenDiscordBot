using KaliskaHaven.Database.Economy;
using KaliskaHaven.Database.Entities;
using KaliskaHaven.Database.Shop;

using Microsoft.EntityFrameworkCore;

using Npgsql.Json.NET;

namespace KaliskaHaven.Database
{
	public class KaliskaDBBackend : DbContext
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

		public DbSet<Person> Persons {
			get; set;
		}


		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(x => { });
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Wallet>(x => {
				x.HasOne(y => y.Owner).WithOne(y => y.Wallet);
				x.HasKey(y => y.ID);
			});
			modelBuilder.Entity<Person>(x => {
				x.HasOne(y => y.Wallet).WithOne(y => y.Owner);
				x.HasKey(y => y.ID);
			});
			/*modelBuilder.Entity<TransactionRecord>(x => {
				x.HasOne(y => y.Deposited);
				x.HasOne(y => y.Withdrawn);
				x.HasOne(y => y.To);
				x.HasOne(y => y.From);
				x.HasKey(y => y.ID);
			});*/
		}
	}

	public class KaliskaDB : KaliskaDBBackend
	{/*
		private readonly KaliskaDBBackend _backend = new KaliskaDBBackend();
		public DbSet<ShopItem> ShopItems => _backend.ShopItems;

		public DbSet<DbCurrency> DbCurrencies => _backend.DbCurrencies;

		public DbSet<TransactionRecord> TransactionRecords => _backend.TransactionRecords;

		public DbSet<Wallet> Wallets => _backend.Wallets;
		public DbSet<Person> Persons => _backend.Persons;
		public SaveChangesAsync*/
	}
}
