using KaliskaHaven.Database.Economy;
using KaliskaHaven.Database.Entities;
using KaliskaHaven.Database.Shop;

using LinqToDB.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

namespace KaliskaHaven.Database
{
	internal class KaliskaDBBackend : DbContext
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

		public KaliskaDBBackend()
		{
		}

		private const string schh = "discordbottie";

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var usr = "KaliskaHavenBot";
			var psswd = "1";
			var db = "Kaliska";

			var cstrs = new LinkedList<string>();
			cstrs.AddLast($"Username={usr}");
			cstrs.AddLast($"Password={psswd}");
			cstrs.AddLast($"Search Path={schh}");
			cstrs.AddLast("Host=localhost");
			cstrs.AddLast("Port=5433");
			cstrs.AddLast($"Database={db}");
			cstrs.AddLast("Include Error Detail=true");
			optionsBuilder.EnableDetailedErrors();
			optionsBuilder.EnableSensitiveDataLogging();
			cstrs.AddLast("Pooling=true");
			cstrs.AddLast("Minimum Pool Size=30");
			cstrs.AddLast("Maximum Pool Size=1000");
			cstrs.AddLast("Connection Lifetime=3600");

			optionsBuilder.UseNpgsql(string.Join(";", cstrs));
			optionsBuilder.UseLinqToDB();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasDefaultSchema(schh);

			modelBuilder.Entity<Wallet>(x => {
				x.HasKey(y => y.ID);
				x.Property(y => y.ID).UseIdentityByDefaultColumn();
			});

			modelBuilder.Entity<Person>(x => {
				x.HasKey(y => y.ID);
				x.Property(y => y.ID).UseIdentityByDefaultColumn();
				x.HasOne(y => y.Wallet).WithOne(y => y.Owner).HasForeignKey<Wallet>(y => y.OwnerID).IsRequired(false);
			});

			modelBuilder.Entity<TransactionRecord>(x => {
				x.HasKey(y => y.ID);
				x.Property(y => y.ID).UseIdentityByDefaultColumn();
				x.HasOne(y => y.Deposited);
				x.HasOne(y => y.Withdrawn);
				x.HasOne(y => y.To);
				x.HasOne(y => y.From);
			});

			modelBuilder.Entity<DbCurrency>(x => {
				x.HasKey(y => y.ID);
				x.Property(y => y.ID).UseIdentityByDefaultColumn();
			});
		}
	}
}
