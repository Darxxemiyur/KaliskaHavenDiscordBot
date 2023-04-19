namespace KaliskaHaven.DiscordClient
{
	/// <summary>
	/// Set of services to be used by modules. Extendable.
	/// </summary>
	public interface IMyServices
	{
		/// <summary>
		/// Returns KaliskaBot if exist. Otherwise doesn't complete the task until it exist.
		/// </summary>
		/// <returns></returns>
		Task<IKaliskaBot> GetKaliskaBot();

		Task<Config> ParseConfiguration();
	}
}
