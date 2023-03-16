namespace KaliskaHaven.Economy;

public sealed class WithdrawResult
{
	/// <summary>
	/// Whether the operation was successful.
	/// </summary>
	public bool IsSuccesful {
		get;
	}

	/// <summary>
	/// Check for WithdrawResult.IsSuccessful.
	/// </summary>
	public Currency? Withdrawn {
		get;
	}

	/// <summary>
	/// The transaction.
	/// </summary>
	public TransactionLog? Transaction {
		get;
	}

	public WithdrawResult()
	{
		IsSuccesful = false;
		Withdrawn = null;
		Transaction = null;
	}

	public WithdrawResult(Currency amount, TransactionLog transaction)
	{
		IsSuccesful = true;
		Withdrawn = amount;
		Transaction = transaction;
	}
}