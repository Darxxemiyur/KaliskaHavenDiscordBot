using DisCatSharp.Entities;
using DisCatSharp.EventArgs;

using KaliskaHaven.Database;
using KaliskaHaven.DiscordClient;
using KaliskaHaven.Glue;
using KaliskaHaven.Glue.Economy;
using KaliskaHaven.Glue.Social;
using KaliskaHaven.SocialModel;

using Name.Bayfaderix.Darxxemiyur.Node.Network;

namespace KaliskaHaven.DiscordUI.EconomyUI;

public sealed class EconomyMenu : GluedNetwork
{
	private readonly ISessionChannel _ch;
	private readonly DiscordUser _user;
	private readonly IGlueServices _gs;

	public EconomyMenu(IGlueServices gs, ISessionChannel channel, DiscordUser user)
	{
		_gs = gs;
		_user = user;
		_ch = channel;
	}

	public override StepInfo GetStartingInstruction() => new(this.Initialize);

	private async Task<StepInfo?> Initialize(StepInfo? prev)
	{
		var context = await _gs.GetKaliskaDB();
		return new StepInfo<KaliskaDB>(this.EntryMenu, context);
	}

	private async Task<StepInfo?> EntryMenu(StepInfo? prev, KaliskaDB db)
	{
		var bt = await _gs.GetKaliskaBot();
		var cl = await bt.GetClient();

		var msg = new UniversalMessageBuilder();
		var wc = await _gs.GetWalletCreator(db);

		msg.AddEmbed(new DiscordEmbedBuilder().WithDescription("Please select operation:"));

		var operations = new (DiscordStringSelectComponentOption Option, Permission Perm, Func<StepInfo?, KaliskaDB, Task<StepInfo?>> Delegate)[] {
			(new DiscordStringSelectComponentOption("Deposit", "deposit"), new EconomyPermission.Deposit(), this.Deposit),
			(new DiscordStringSelectComponentOption("Withdraw", "withdraw"), new EconomyPermission.Withdraw(), this.Withdraw),
			(new DiscordStringSelectComponentOption("Transfer", "transfer"), new EconomyPermission.Transfer(), this.Transfer),
			(new DiscordStringSelectComponentOption("Convert", "convert"), new  EconomyPermission.Convert(), this.Convert)
		};

		msg.AddComponents(new DiscordStringSelectComponent("Operation:", operations.Select(x => x.Option)));

		await _ch.SendMessage(msg);

		var sre = await _ch.GetSessionRelatedEvents<InteractionCreateEventArgs>();
		using var tokenS = new CancellationTokenSource();
		tokenS.CancelAfter(TimeSpan.FromSeconds(10));
		try
		{
			await sre.GetItem(tokenS.Token);
		}
		catch (TaskCanceledException)
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

		var perms = await user.Permissions.Acquire();
		if (perms?.Identifyable != null)
			foreach (var perm in perms.Identifyable)
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

	private async Task<StepInfo?> Convert(StepInfo? prev, KaliskaDB db)
	{
		throw new NotImplementedException();
	}

	public override Task<GluedNetworkTransferPayload> ArchiveNetwork() => throw new NotImplementedException();
	public override Task<bool> IsArchived() => throw new NotImplementedException();
	public override Task<bool> UnArchive(GluedNetworkTransferPayload payload) => throw new NotImplementedException();
	public override Task<bool> Commit() => throw new NotImplementedException();
}
