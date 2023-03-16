using KaliskaHaven.Database;
using KaliskaHaven.Database.Economy;
using KaliskaHaven.Database.Entities;
using KaliskaHaven.Economy;

using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Glue.Economy
{
	public sealed class Wallet : IDbWallet
	{
		private readonly KaliskaDB _db;
		private readonly Database.Economy.Wallet _wallet;

		public Wallet(KaliskaDB db, Database.Economy.Wallet wallet)
		{
			_db = db;
			_wallet = wallet;
		}

		public ulong ID => _wallet.ID;

		public ICollection<DbCurrency> DbCurrencies => _wallet.DbCurrencies;

		public Person Owner => _wallet.Owner;

		public IIdentity? Identity => _wallet.Identity;

		public IWallet? Identifyable => _wallet.Identifyable;

		public Type Type => _wallet.Type;

		public async Task EnsureFullyLoaded()
		{
			var entry = _db.Entry(_wallet);
			await entry.Collection(x => x.DbCurrencies).LoadAsync();
			await entry.Reference(x => x.Owner).LoadAsync();
			await _db.SaveChangesAsync();
		}

		public async Task<IIdentifiable<ITransactionLog>> Deposit(Currency currency)
		{
			await this.EnsureFullyLoaded();
			var transaction = await _wallet.Deposit(currency);
			await _db.SaveChangesAsync();
			return transaction;
		}

		public bool Equals<TId>(IIdentifiable<TId> to) => to is IIdentifiable<IWallet> wa && wa is IDbWallet dw && dw.ID == ID;

		public async Task<Currency?> Get(CurrencyType currency)
		{
			await this.EnsureFullyLoaded();
			return await _wallet.Get(currency);
		}

		public async IAsyncEnumerable<Currency> GetAll()
		{
			await this.EnsureFullyLoaded();
			await foreach (var curr in _wallet.GetAll())
				yield return curr;
		}

		public async Task<IIdentifiable<ITransactionLog>> Withdraw(Currency currency)
		{
			await this.EnsureFullyLoaded();
			var transaction = await _wallet.Withdraw(currency);
			await _db.SaveChangesAsync();
			return transaction;
		}
	}
}
