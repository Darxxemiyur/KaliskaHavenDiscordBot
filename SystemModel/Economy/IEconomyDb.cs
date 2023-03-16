namespace KaliskaHaven.Economy;

public interface IEconomyDb
{
	public Task UpdateWallet(IWallet wallet);

	public Task LogTransaction(TransactionLog log);
}