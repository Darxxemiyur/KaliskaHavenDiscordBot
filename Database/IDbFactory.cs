using KaliskaHaven.DiscordBot;

namespace KaliskaHaven.DiscordBot.src.DataStorage
{
	public interface IDbFactory<Database> where Database : class
	{
		Task PrepareFactory(object services);

		Task<Database> BuildDBAsync();
	}
}