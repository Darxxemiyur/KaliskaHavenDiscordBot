using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Economy;

public sealed class TransactionLog : ITransactionLog
{
	public TranscationKind Kind {
		get; set;
	}

	public IIdentifiable<IWallet>? From {
		get; set;
	}

	public IIdentifiable<IWallet>? To {
		get; set;
	}

	public Currency? Withdrawn {
		get; set;
	}

	public Currency? Deposited {
		get; set;
	}

	public TransactionLog() => Kind = TranscationKind.FailedGeneral;

	public TransactionLog(TranscationKind kind, IIdentifiable<IWallet> wallet, Currency quantity)
	{
		if (kind is TranscationKind.Withdrawal or TranscationKind.FailedWithdrawal)
		{
			From = wallet;
			Withdrawn = quantity;
		}
		else if (kind is TranscationKind.Deposit or TranscationKind.FailedDeposit)
		{
			To = wallet;
			Deposited = quantity;
		}
		else
		{
			throw new InvalidOperationException();
		}
		Kind = kind;
	}

	public TransactionLog(TranscationKind kind, IIdentifiable<IWallet> from, IIdentifiable<IWallet> to, Currency withdrawn, Currency deposited)
	{
		if (kind is not TranscationKind.Transfer and not TranscationKind.Exchange and not TranscationKind.FailedTransfer and not TranscationKind.FailedExchange)
			throw new InvalidOperationException();
		Kind = kind;
		From = from;
		To = to;
		Withdrawn = withdrawn;
		Deposited = deposited;
	}

	public TransactionLog(ITransactionLog log)
	{
		Kind = log.Kind;
		From = log.From;
		To = log.To;
		Withdrawn = log.Withdrawn;
		Deposited = log.Deposited;
	}
}
