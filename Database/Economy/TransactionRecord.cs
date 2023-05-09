using KaliskaHaven.Economy;

using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Database.Economy
{
	public sealed class TransactionRecord : TransactionLog, ITransactionLog, IIdentifiable<ITransactionLog>
	{
		public long ID {
			get; set;
		}

		public override TranscationKind Kind {
			get; set;
		}

		public override IIdentifiable<IWallet>? From {
			get; set;
		}

		public override IIdentifiable<IWallet>? To {
			get; set;
		}

		public Wallet? FromW {
			get; set;
		}

		public Wallet? ToW {
			get; set;
		}

		IIdentifiable<IWallet>? ITransactionLog.From => From;

		IIdentifiable<IWallet>? ITransactionLog.To => To;

		public DbCurrency? Withdrawn {
			get; set;
		}

		IIdentifiable<Currency>? ITransactionLog.Withdrawn => Withdrawn;

		public DbCurrency? Deposited {
			get; set;
		}

		IIdentifiable<Currency>? ITransactionLog.Deposited => Deposited;

		public IIdentity? Identity => throw new NotImplementedException();
		public ITransactionLog? Identifyable => this;

		public TransactionRecord()
		{
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3358:Ternary operators should not be nested", Justification = "Fuck you.")]
		public TransactionRecord(ITransactionLog log)
		{
			Kind = log.Kind;
			From = log.From == null ? null : (log.From is Wallet fr ? fr : new Wallet(log.From));
			To = log.To == null ? null : (log.To is Wallet to ? to : new Wallet(log.To));
			Withdrawn = log.Withdrawn == null ? null : log.Withdrawn is DbCurrency w ? w : new DbCurrency(log.Withdrawn);
			Deposited = log.Deposited == null ? null : log.Deposited is DbCurrency d ? d : new DbCurrency(log.Deposited);
		}

		public Type Type {
			get;
		} = typeof(TransactionRecord);

		public bool Equals<TId>(IIdentifiable<TId> to) => to is TransactionRecord tr && tr.ID == ID;
	}
}
