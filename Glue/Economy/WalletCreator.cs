using DisCatSharp.Entities;

using KaliskaHaven.Database;
using KaliskaHaven.Database.Entities;

using LinqToDB;

namespace KaliskaHaven.Glue.Economy
{
	public sealed class WalletCreator
	{
		private readonly WalletCreatorArgs _args;

		public WalletCreator(WalletCreatorArgs args) => _args = args;

		public async Task<(Person, Wallet)> EnsureCreated(DiscordUser user)
		{
			var uc = await _args.Services.GetUserCreator(new Social.UserCreatorArgs(_args.Services, _args.KaliskaDB));
			var person = await uc.EnsureCreated(user);
			var wallet = await this.EnsureCreated(person);
			await wallet.EnsureFullyLoaded();

			return (person, wallet);
		}

		public async Task<Wallet> EnsureCreated(Person person)
		{
			var db = _args.KaliskaDB ?? await _args.Services.GetKaliskaDB();
			var walletT = await db.Wallets.FirstOrDefaultAsync(x => x.Owner == person);

			await using var tr = await db.Database.BeginTransactionAsync();
			if (walletT != null)
			{
				await tr.CommitAsync();
				return new Wallet(db, walletT);
			}

			var wallet = new Database.Economy.Wallet();
			person.Wallet = wallet;
			wallet.Owner = person;

			await db.Wallets.AddAsync(wallet);
			await db.SaveChangesAsync();

			await tr.CommitAsync();

			return new Wallet(db, wallet);
		}
	}

	public sealed record class WalletCreatorArgs(IGlueServices Services, KaliskaDB? KaliskaDB);
}
