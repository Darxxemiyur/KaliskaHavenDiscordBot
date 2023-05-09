using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Economy
{
	public interface ITransactionLog
	{
		public TranscationKind Kind {
			get;
		}

		public IIdentifiable<IWallet>? From {
			get;
		}

		public IIdentifiable<IWallet>? To {
			get;
		}

		public IIdentifiable<Currency>? Withdrawn {
			get;
		}

		public IIdentifiable<Currency>? Deposited {
			get;
		}
	}
}
