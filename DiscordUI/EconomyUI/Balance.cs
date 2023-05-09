using DisCatSharp.Entities;
using DisCatSharp.EventArgs;

using KaliskaHaven.Database;
using KaliskaHaven.Database.Economy;
using KaliskaHaven.DiscordClient;
using KaliskaHaven.DiscordClient.SessionChannels;
using KaliskaHaven.Glue;
using KaliskaHaven.Glue.Economy;

using Name.Bayfaderix.Darxxemiyur.Node.Network;

namespace KaliskaHaven.DiscordUI.EconomyUI
{
	public sealed class Balance : INodeNetwork
	{
		private readonly BareMessageChannel _ch;
		private readonly DiscordUser _user;
		private readonly IGlueServices _gs;

		public Balance(IGlueServices gs, BareMessageChannel channel, DiscordUser user)
		{
			_gs = gs;
			_user = user;
			_ch = channel;
		}

		public StepInfo GetStartingInstruction() => new(this.EntryMenu);

		private async Task<StepInfo?> EntryMenu(StepInfo? prev)
		{
			var usri = (ulong)70349108077924352;
			var bt = await _gs.GetKaliskaBot();
			var cl = await bt.GetClient();
			var usr = await cl.GetUserAsync(usri);

			await using var context = await _gs.GetKaliskaDB();
			var msg = new UniversalMessageBuilder();
			var wc = await _gs.GetWalletCreator(context);

			var (person, wallet) = await wc.EnsureCreated(_user);
			var (person2, wallet2) = await wc.EnsureCreated(usr);

			await wallet.Deposit(new Economy.Currency(Economy.CurrencyType.Kelpie_Cash, 100));
			await wallet.Deposit(new Economy.Currency(Economy.CurrencyType.Kelpie_Cash, 100));
			await wallet.Transfer(wallet2, new Economy.Currency(Economy.CurrencyType.Kelpie_Cash, 142));

			msg.SetContent(GetCurrencyList(wallet.DbCurrencies));

			await _ch.SendMessage(msg);
			return null;
		}

		private string GetCurrencyList(IEnumerable<DbCurrency> currs) => string.Join("\n", currs.Select(x => $"{x.CurrencyType} - {x.Quantity}") is var fs && fs.Any() ? fs : new string[] { "No currencies." });
	}
}
