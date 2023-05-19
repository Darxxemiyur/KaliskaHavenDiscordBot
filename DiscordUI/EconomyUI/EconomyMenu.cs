using DisCatSharp.Entities;
using DisCatSharp.EventArgs;

using KaliskaHaven.Database;
using KaliskaHaven.Database.Economy;
using KaliskaHaven.DiscordClient;
using KaliskaHaven.DiscordClient.SessionChannels;
using KaliskaHaven.Glue;
using KaliskaHaven.Glue.Economy;
using KaliskaHaven.Glue.Social;
using KaliskaHaven.SocialModel;

using Name.Bayfaderix.Darxxemiyur.Node.Network;

namespace KaliskaHaven.DiscordUI.EconomyUI;

public sealed class EconomyMenu : INodeNetwork
{
	private readonly BareMessageChannel _ch;
	private readonly DiscordUser _user;
	private readonly IGlueServices _gs;

	public EconomyMenu(IGlueServices gs, BareMessageChannel channel, DiscordUser user)
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

		msg.AddEmbed(new DiscordEmbedBuilder().WithDescription("Please select operation:"));

		var operations = new (DiscordStringSelectComponentOption, Permission)[] {
			(new DiscordStringSelectComponentOption("Deposit", "deposit"), new EconomyPermission.Deposit()),
			(new DiscordStringSelectComponentOption("Withdraw", "withdraw"), new EconomyPermission.Withdraw()),
			(new DiscordStringSelectComponentOption("Transfer", "transfer"), new EconomyPermission.Transfer()),
			(new DiscordStringSelectComponentOption("Convert","convert"), new  EconomyPermission.Convert())
		};

		msg.AddComponents(new DiscordStringSelectComponent("Operation:", operations.Select(x => x.Item1)));

		await _ch.SendMessage(msg);

		var sre = await _ch.GetSessionRelatedEvents<InteractionCreateEventArgs>();
		using var tokenS = new CancellationTokenSource();
		tokenS.CancelAfter(TimeSpan.FromSeconds(10));
		try
		{
			await sre.GetItem(tokenS.Token);
		}
		catch (TaskCanceledException e)
		{
			await _ch.SendMessage("Timed out.\nSession closed.");
			return null;
		}

		return new StepInfo<KaliskaDB, StepInfo, StepInfo, Permission>(this.CheckPermissions, db, new StepInfo<KaliskaDB>(this.Deposit, db), new StepInfo<KaliskaDB>(this.EntryMenu, db), new EconomyPermission.Deposit());
	}
	private async Task<StepInfo?> CheckPermissions(StepInfo? prev, KaliskaDB db, StepInfo onSuccess, StepInfo onFail, Permission permReq)
	{
		var uc = await _gs.GetUserCreator(new UserCreatorArgs(_gs, db));
		var user = await uc.EnsureCreated(_user);

		await foreach (var perm in user.Permissions)
			if (perm == permReq)
				return onSuccess;
		return onFail;
	}
	private async Task<StepInfo?> Deposit(StepInfo? prev, KaliskaDB db)
	{
		throw new NotImplementedException();
	}
	private async Task<StepInfo?> Withdraw(StepInfo? prev, KaliskaDB db)
	{
		throw new NotImplementedException();
	}
	private async Task<StepInfo?> Transfer(StepInfo? prev, KaliskaDB db)
	{
		throw new NotImplementedException();
	}
}
