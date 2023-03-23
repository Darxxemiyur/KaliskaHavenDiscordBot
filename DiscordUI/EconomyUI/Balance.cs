using DisCatSharp.Entities;

using KaliskaHaven.Database;
using KaliskaHaven.DiscordClient.SessionChannels;
using KaliskaHaven.Glue.Economy;

using Name.Bayfaderix.Darxxemiyur.Node.Network;

namespace KaliskaHaven.DiscordUI.EconomyUI
{
	public sealed class Balance : INodeNetwork
	{
		private readonly BareMessageChannel _ch;
		private readonly DiscordUser _user;

		public Balance(BareMessageChannel channel, DiscordUser user)
		{
			_user = user;
			_ch = channel;
		}

		public StepInfo GetStartingInstruction() => new(EntryMenu);

		private async Task<StepInfo?> EntryMenu(StepInfo? prev)
		{
			await using var context = new KaliskaDB();
			var embs = new DiscordEmbedBuilder();

			var (person, wallet) = await Wallet.EnsureCreated(context, _user);

			await wallet.EnsureFullyLoaded();

			embs.AddField("Balance:", GetCurrencyList(wallet.DbCurrencies));
			await _ch.SendMessage(embs);

			return null;
		}

		private string GetCurrencyList(IEnumerable<Database.Economy.DbCurrency> currs) => string.Join("\n", currs.Select(x => $"{x.CurrencyType} - {x.Quantity}") is var fs && fs.Any() ? fs : new string[] { "No currencies." });
	}
}
