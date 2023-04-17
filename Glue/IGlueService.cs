using KaliskaHaven.Database;
using KaliskaHaven.Glue.Economy;
using KaliskaHaven.Glue.Social;

namespace KaliskaHaven.Glue;

/// <summary>
/// Interface of the services provider for the Glue layer.
/// </summary>
public interface IGlueService
{
	/// <summary>
	/// Acquire Kaliska's Database.
	/// </summary>
	/// <returns></returns>
	Task<KaliskaDB> GetKaliskaDB();

	/// <summary>
	/// Get wallet creator.
	/// </summary>
	/// <returns></returns>
	Task<WalletCreator> GetWalletCreator(KaliskaDB db);

	/// <summary>
	/// Get user creator.
	/// </summary>
	/// <returns></returns>
	Task<UserCreator> GetUserCreator();
}
