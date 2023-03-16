using KaliskaHaven.Economy;

using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Database.Economy
{
	public sealed class TransactionRecord : ITransactionLog, IIdentifiable<ITransactionLog>
	{
		public ulong ID {
			get; set;
		}

		public TranscationKind Kind {
			get; set;
		}

		public Wallet? From {
			get; set;
		}
		public Wallet? To {
			get; set;
		}

		IIdentifiable<IWallet>? ITransactionLog.From {
			get;
		}

		IIdentifiable<IWallet>? ITransactionLog.To {
			get;
		}

		public DbCurrency? Withdrawn {
			get; set;
		}

		Currency? ITransactionLog.Withdrawn => Withdrawn;

		public DbCurrency? Deposited {
			get; set;
		}

		Currency? ITransactionLog.Deposited => Deposited;

		public IIdentity? Identity => throw new NotImplementedException();
		public ITransactionLog? Identifyable => this;

		public TransactionRecord(ITransactionLog log)
		{
			Kind = log.Kind;
			From = log.From == null ? null : new Wallet(log.From);
			To = log.To == null ? null : new Wallet(log.To);
			Withdrawn = log.Withdrawn == null ? null : new DbCurrency(log.Withdrawn);
			Deposited = log.Deposited == null ? null : new DbCurrency(log.Deposited);
		}

		public Type Type {
			get;
		} = typeof(TransactionRecord);

		public bool Equals<TId>(IIdentifiable<TId> to) => to is TransactionRecord tr && tr.ID == ID;
	}
}
