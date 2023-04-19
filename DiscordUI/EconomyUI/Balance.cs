using DisCatSharp.Entities;
using DisCatSharp.EventArgs;

using KaliskaHaven.Database;
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
			await using var context = await _gs.GetKaliskaDB();
			var msg = new UniversalMessageBuilder();
			var wc = await _gs.GetWalletCreator(context);

			var (person, wallet) = await wc.EnsureCreated(_user);

			await wallet.EnsureFullyLoaded();
			var usr = await _user.GetFromApiAsync();

			var userAvatar = usr.GetAvatarUrl(DisCatSharp.ImageFormat.Auto);
			using var wla = await Wallet.GetWalletImage(wallet, this.GetCurrencyList(wallet.DbCurrencies), usr.UsernameWithDiscriminator, userAvatar);
			var ses = await _ch.GetSessionRelatedEvents<MessageCreateEventArgs>();
			var getter = ses.GetItem();
			var fname = string.Join('.', "Avatar", userAvatar.Split('.').LastOrDefault()?.Split('?')?.FirstOrDefault());
			await _ch.SendMessage(new UniversalMessageBuilder().SetFile(fname, wla));
			var imgUrl = (await getter).Message.Attachments[0].ProxyUrl;
			msg.AddEmbeds(new DiscordEmbedBuilder().WithThumbnail(userAvatar)
				.WithImageUrl(imgUrl).WithColor(new DiscordColor(210, 190, 30))
				.AddField(new DiscordEmbedField("User:", $"<@{person.DiscordId}>"))
				.AddField(new DiscordEmbedField("Balance:", this.GetCurrencyList(wallet.DbCurrencies))));
			await _ch.SendMessage(msg);

			return null;
		}

		private string GetCurrencyList(IEnumerable<Database.Economy.DbCurrency> currs) => string.Join("\n", currs.Select(x => $"{x.CurrencyType} - {x.Quantity}") is var fs && fs.Any() ? fs : new string[] { "No currencies." });
	}
}
