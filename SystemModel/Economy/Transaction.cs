using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaliskaHaven.Economy
{
	/// <summary>
	/// Performed transaction kind.
	/// </summary>
	public enum TranscationKind
	{
		/// <summary>
		/// Deposit transaction performed.
		/// </summary>
		Deposit,
		/// <summary>
		/// Withdrawal transaction performed.
		/// </summary>
		Withdrawal,
		/// <summary>
		/// Exchange transaction performed.
		/// </summary>
		Exchange,
		/// <summary>
		/// Transfer transaction performed.
		/// </summary>
		Transfer,
		/// <summary>
		/// Transaction created, but has no effect.
		/// </summary>
		FailedGeneral,
		/// <summary>
		/// Transaction created, but has no effect.
		/// </summary>
		FailedDeposit,
		/// <summary>
		/// Transaction created, but has no effect.
		/// </summary>
		FailedWithdrawal,
		/// <summary>
		/// Transaction created, but has no effect.
		/// </summary>
		FailedExchange,
		/// <summary>
		/// Transaction created, but has no effect.
		/// </summary>
		FailedTransfer,
	}
	public sealed class TransactionLog
	{
		public TranscationKind Kind {
			get;
		}
		public IWallet? From {
			get;
		}
		public IWallet? To {
			get;
		}
		public Currency? Withdrawn {
			get;
		}
		public Currency? Deposited {
			get;
		}
		public TransactionLog() => Kind = TranscationKind.FailedGeneral;
		public TransactionLog(TranscationKind kind, IWallet wallet, Currency quantity)
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
		public TransactionLog(TranscationKind kind, IWallet from, IWallet to, Currency withdrawn, Currency deposited)
		{
			if (kind is not TranscationKind.Transfer and not TranscationKind.Exchange and not TranscationKind.FailedTransfer and not TranscationKind.FailedExchange)
				throw new InvalidOperationException();
			Kind = kind;
			From = from;
			To = to;
			Withdrawn = withdrawn;
			Deposited = deposited;
		}
	}
}
