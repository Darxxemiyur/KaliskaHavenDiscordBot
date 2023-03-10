using Microsoft.EntityFrameworkCore;

namespace KaliskaHaven.DiscordBot.src.DataStorage
{
	public class Pgsqldb : DbContext
	{

		public Pgsqldb(DbContextOptions options) : base(options)
		{

		}
	}
}