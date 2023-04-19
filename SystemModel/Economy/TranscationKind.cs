namespace KaliskaHaven.Economy;

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
