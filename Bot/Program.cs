using DisCatSharp.EventArgs;

using KaliskaHaven.Database;
using KaliskaHaven.DiscordClient;
using KaliskaHaven.DiscordClient.SessionChannels;
using KaliskaHaven.DiscordUI.EconomyUI;

using Microsoft.EntityFrameworkCore;

using Name.Bayfaderix.Darxxemiyur.Common;
using Name.Bayfaderix.Darxxemiyur.Common.Async;
using Name.Bayfaderix.Darxxemiyur.Common.Extensions;
using Name.Bayfaderix.Darxxemiyur.Node.Network;

using System.Configuration;

namespace KaliskaHaven.Bot
{
	internal class Program : IAsyncRunnable
	{
		private readonly MyServices _services;

		public Program()
		{
			_services = new();
			var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoaming);
		}

		public async Task RunRunnable(CancellationToken token = default)
		{
			await _services.Initialize();

			var sc = new MySingleThreadSyncContext();

			await await MyTaskExtensions.RunOnScheduler(() => Task.WhenAny(_services.RunRunnable(), RunTest()), scheduler: await sc.MyTaskSchedulerPromise);
		}

		private async Task RunTest()
		{
			var kali = await _services.GetKaliskaBot();

			var ch = new BareMessageChannel(kali, 1083244585410637885, null);
			var bot = await _services.GetKaliskaBot();
			var r = await bot.GetEventRouter<ReadyEventArgs>();

			var reb = await r.PlaceRequest();

			await reb.GetItem();

			var p = await (await bot.GetClient()).GetUserAsync(860897395109789706);

			var net = new Balance(ch, p) as INodeNetwork;

			await using (var db = new KaliskaDB())
			{
				await db.Database.MigrateAsync();
			}

			await net.RunNetwork();
		}

		private static async Task Main(string[] args)
		{
			var prog = new Program();
			await prog.RunRunnable();
		}
	}
}
