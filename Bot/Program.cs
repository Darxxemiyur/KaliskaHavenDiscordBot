using DisCatSharp.EventArgs;

using KaliskaHaven.DiscordClient;
using KaliskaHaven.DiscordClient.SessionChannels;
using KaliskaHaven.DiscordUI.EconomyUI;

using Microsoft.EntityFrameworkCore;

using Name.Bayfaderix.Darxxemiyur.Async;
using Name.Bayfaderix.Darxxemiyur.Extensions;
using Name.Bayfaderix.Darxxemiyur.Node.Network;
using Name.Bayfaderix.Darxxemiyur.Tasks;

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
			await using (var db = await _services.GetKaliskaDB())
				await db.Database.MigrateAsync();

			await _services.Initialize();

			var sc = new MySingleThreadSyncContext();

			await await MyTaskExtensions.RunOnScheduler(() => Task.WhenAny(_services.RunRunnable(), this.RunTest()), scheduler: await sc.MyTaskSchedulerPromise);
		}

		private async Task RunTest()
		{
			var kali = await _services.GetKaliskaBot();

			var ch = new BareMessageChannel(kali, 1083244585410637885, null);
			var r = await kali.GetEventRouter<ReadyEventArgs>();

			var reb = await r.PlaceRequest();

			await reb.GetItem();

			var p = await (await kali.GetClient()).GetUserAsync(860897395109789706);

			var net = new EconomyMenu(_services, ch, p) as INodeNetwork;

			await net.RunNetwork();
		}

		private static async Task Main(string[] args)
		{
			var prog = new Program();
			await prog.RunRunnable();
		}
	}
}
