using KaliskaHaven.Database.Entities;
using KaliskaHaven.Economy;

using Name.Bayfaderix.Darxxemiyur.Extensions;
using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Database.Economy
{
	public sealed class Wallet : IDbWallet
	{
		public long ID {
			get; set;
		}

		public ICollection<DbCurrency> DbCurrencies {
			get; set;
		}

		public Person? Owner {
			get; set;
		}

		public long? OwnerID {
			get; set;
		}

		public IIdentity? Identity => throw new NotImplementedException();
		public IWallet? Identifyable => this;

		public Type Type {
			get;
		} = typeof(Wallet);

		public bool Equals<TId>(IIdentifiable<TId> to) => to is Wallet bw && bw.ID == ID;

		public Wallet()
		{
		}

		public Wallet(IIdentifiable<IWallet> wallet)
		{
			if (wallet is not IDbWallet bw)
				throw new NotImplementedException();

			ID = bw.ID;
			DbCurrencies = bw.DbCurrencies;
			Owner = bw.Owner;
		}

		public async Task<IIdentifiable<ITransactionLog>> Deposit(Currency currency) => await MyTaskExtensions.RunOnScheduler(() => {
			if (DbCurrencies == null)
				throw new InvalidOperationException();

			var curr = DbCurrencies.FirstOrDefault(x => x.CurrencyType == currency.CurrencyType);

			if (curr == null)
				DbCurrencies.Add(curr = new DbCurrency(currency.CurrencyType, 0));

			curr.Quantity += currency.Quantity;

			return new TransactionRecord(new TransactionLog(TranscationKind.Deposit, this, new DbCurrency(currency)));
		});

		public Task<Currency?> Get(CurrencyType currency) => MyTaskExtensions.RunOnScheduler(() => {
			if (DbCurrencies == null)
				throw new InvalidOperationException();

			return DbCurrencies.FirstOrDefault(x => x.CurrencyType == currency) as Currency;
		});

		public async IAsyncEnumerable<Currency> GetAllCurrencies()
		{
			if (DbCurrencies == null)
				throw new InvalidOperationException();

			foreach (var currency in DbCurrencies)
				yield return currency;

			await Task.CompletedTask;
		}

		public async Task<IIdentifiable<ITransactionLog>> Withdraw(Currency currency) => new TransactionRecord(await MyTaskExtensions.RunOnScheduler(() => {
			if (DbCurrencies == null)
				throw new InvalidOperationException();

			var curr = DbCurrencies.FirstOrDefault(x => x.CurrencyType == currency.CurrencyType);

			var freshCurrency = new DbCurrency(currency);

			if (curr == null || curr.Quantity < currency.Quantity)
				return new TransactionLog(TranscationKind.FailedWithdrawal, this, freshCurrency);

			curr.Quantity -= currency.Quantity;

			return new TransactionLog(TranscationKind.Withdrawal, this, freshCurrency);
		}));

		public async Task<IIdentifiable<ITransactionLog>> Transfer(IDbWallet receiver, Currency currency) => new TransactionRecord(await MyTaskExtensions.RunOnScheduler(() => {
			if (DbCurrencies == null)
				throw new InvalidOperationException();
			var receiverCurr = receiver.DbCurrencies.FirstOrDefault(x => x.CurrencyType == currency.CurrencyType);
			var senderCurr = DbCurrencies.FirstOrDefault(x => x.CurrencyType == currency.CurrencyType);

			var freshCurrency = new DbCurrency(currency);

			if (senderCurr == null || senderCurr.Quantity < currency.Quantity)
				return new TransactionLog(TranscationKind.FailedTransfer, this, receiver, freshCurrency, freshCurrency);

			if (receiverCurr == null)
				receiver.DbCurrencies.Add(receiverCurr = new DbCurrency(currency.CurrencyType, 0));

			senderCurr.Quantity -= currency.Quantity;
			receiverCurr.Quantity += currency.Quantity;

			return new TransactionLog(TranscationKind.Transfer, this, receiver, freshCurrency, freshCurrency);
		}));
	}
}
