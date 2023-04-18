using KaliskaHaven.Database.Economy;
using KaliskaHaven.Database.Entities;
using KaliskaHaven.Database.Shop;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

using Name.Bayfaderix.Darxxemiyur.Extensions;

namespace KaliskaHaven.Database
{
	public sealed class KaliskaDB : IAsyncDisposable, IDisposable
	{
		#region Facade thingy

		public DbSet<ShopItem> ShopItems => _node.DBBackend.ThrowIfNull().ShopItems;

		public DbSet<DbCurrency> DbCurrencies => _node.DBBackend.ThrowIfNull().DbCurrencies;

		public DbSet<TransactionRecord> TransactionRecords => _node.DBBackend.ThrowIfNull().TransactionRecords;

		public DbSet<Wallet> Wallets => _node.DBBackend.ThrowIfNull().Wallets;
		public DbSet<Person> Persons => _node.DBBackend.ThrowIfNull().Persons;

		public DatabaseFacade Database => _node.DBBackend.ThrowIfNull().Database;

		internal KaliskaDB(DBCacheNode node) => _node = node;

		private readonly DBCacheNode _node;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "It does call SuppressFinalize. Look closer.")]
		public async ValueTask DisposeAsync() => await Task.Run(this.Dispose);

		~KaliskaDB() => _node.Finalization.Call();

		public Task<int> SaveChangesAsync(CancellationToken token = default) => _node.DBBackend.ThrowIfNull().SaveChangesAsync(token);

		public EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class => _node.DBBackend.ThrowIfNull().Entry(entity);

		public void Dispose()
		{
			_node.Finalization.Call();
			GC.SuppressFinalize(this);
		}

		#endregion Facade thingy
	}
}
