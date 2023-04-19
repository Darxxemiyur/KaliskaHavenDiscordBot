using DisCatSharp.Entities;

using KaliskaHaven.Database;
using KaliskaHaven.Database.Entities;

using Microsoft.EntityFrameworkCore;

namespace KaliskaHaven.Glue.Social
{
	public sealed class UserCreator
	{
		private readonly UserCreatorArgs _args;

		public UserCreator(UserCreatorArgs args) => _args = args;
		public async Task<Person> EnsureCreated(DiscordUser user)
		{
			var db = _args.KaliskaDB ?? await _args.Services.GetKaliskaDB();
			var person = await db.Persons.FirstOrDefaultAsync(x => x.DiscordId == user.Id);
			if (person == null)
			{
				person = new Person {
					DiscordId = user.Id,
				};

				await using var tr = await db.Database.BeginTransactionAsync();
				await db.Persons.AddAsync(person);
				await db.SaveChangesAsync();
				await tr.CommitAsync();
			}

			return person;
		}
	}
	public sealed record class UserCreatorArgs(IGlueServices Services, KaliskaDB? KaliskaDB);
}
