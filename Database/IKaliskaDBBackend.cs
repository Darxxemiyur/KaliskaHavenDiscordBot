using KaliskaHaven.Database.Economy;
using KaliskaHaven.Database.Entities;
using KaliskaHaven.Database.Shop;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace KaliskaHaven.Database;

public interface IKaliskaDBBackend : IAsyncDisposable, IDisposable
{
	DbSet<DbCurrency> DbCurrencies {
		get;
	}

	DbSet<Person> Persons {
		get;
	}

	DbSet<ShopItem> ShopItems {
		get;
	}

	DbSet<TransactionRecord> TransactionRecords {
		get;
	}

	DbSet<Wallet> Wallets {
		get;
	}

	Task<int> SaveChangesAsync(CancellationToken token = default);

	EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

	DatabaseFacade Database {
		get;
	}
}
