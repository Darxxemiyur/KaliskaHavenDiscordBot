using DisCatSharp.Entities;

using KaliskaHaven.Database;
using KaliskaHaven.Database.Economy;
using KaliskaHaven.DiscordClient;
using KaliskaHaven.DiscordClient.SessionChannels;
using KaliskaHaven.Glue;
using KaliskaHaven.Glue.Social;

using Name.Bayfaderix.Darxxemiyur.Node.Network;

namespace KaliskaHaven.DiscordUI.EconomyUI;

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

	public StepInfo GetStartingInstruction() => new(this.Initialize);

	private async Task<StepInfo?> Initialize(StepInfo? prev)
	{
		var context = await _gs.GetKaliskaDB();
		return new StepInfo<KaliskaDB>(this.EntryMenu, context);
	}

	private async Task<StepInfo?> EntryMenu(StepInfo? prev, KaliskaDB db)
	{
		var usri = (ulong)70349108077924352;
		var bt = await _gs.GetKaliskaBot();
		var cl = await bt.GetClient(); 

		var msg = new UniversalMessageBuilder();
		var wc = await _gs.GetWalletCreator(db);

		await _ch.SendMessage(msg);
		return new StepInfo<KaliskaDB, StepInfo, StepInfo>(this.CheckPermissions, db, new StepInfo<KaliskaDB>(this.Deposit, db), new StepInfo<KaliskaDB>(this.EntryMenu, db));
	}
	private async Task<StepInfo?> CheckPermissions(StepInfo? prev, KaliskaDB db, StepInfo onSuccess, StepInfo onFail)
	{
		var uc = await _gs.GetUserCreator(new UserCreatorArgs(_gs, db));
		var user = await uc.EnsureCreated(_user);

		//Have not yet decided how to check.
		return _user.IsCurrent ? onSuccess : onFail;
	}
	private async Task<StepInfo?> Deposit(StepInfo? prev, KaliskaDB db)
	{

	}
	private string GetCurrencyList(IEnumerable<DbCurrency> currs) => string.Join("\n", currs.Select(x => $"{x.CurrencyType} - {x.Quantity}") is var fs && fs.Any() ? fs : new string[] { "No currencies." });
}
