using DisCatSharp.Entities;

using KaliskaHaven.Database;
using KaliskaHaven.Database.Entities;

using LinqToDB;

namespace KaliskaHaven.Glue.Economy
{
	public sealed class WalletCreator
	{
		private readonly KaliskaDB _db;

		public WalletCreator(KaliskaDB db) => _db = db;

		public async Task<(Person, Wallet)> EnsureCreated(DiscordUser user)
		{
			var person = await _db.Persons.FirstOrDefaultAsync(x => x.DiscordId == user.Id);
			if (person == null)
			{
				person = new Person {
					DiscordId = user.Id,
				};

				await using var tr = await _db.Database.BeginTransactionAsync();
				await _db.Persons.AddAsync(person);
				await _db.SaveChangesAsync();
				await tr.CommitAsync();
			}

			var wallet = await this.EnsureCreated(person);

			return (person, wallet);
		}

		public async Task<Wallet> EnsureCreated(Person person)
		{
			var walletT = await _db.Wallets.FirstOrDefaultAsync(x => x.Owner == person);

			await using var tr = await _db.Database.BeginTransactionAsync();
			if (walletT != null)
			{
				await tr.CommitAsync();
				return new Wallet(_db, walletT);
			}

			var wallet = new Database.Economy.Wallet();
			person.Wallet = wallet;
			wallet.Owner = person;

			await _db.Wallets.AddAsync(wallet);
			await _db.SaveChangesAsync();

			await tr.CommitAsync();

			return new Wallet(_db, wallet);
		}
	}
}
