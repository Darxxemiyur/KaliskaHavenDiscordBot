using Name.Bayfaderix.Darxxemiyur.General;

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
	public IIdentifiable<Currency>? Withdrawn => Transaction?.Withdrawn;

	/// <summary>
	/// The transaction.
	/// </summary>
	public ITransactionLog? Transaction {
		get;
	}

	public WithdrawResult()
	{
		IsSuccesful = false;
		Transaction = null;
	}

	public WithdrawResult(ITransactionLog transaction)
	{
		IsSuccesful = true;
		Transaction = transaction;
	}
}
