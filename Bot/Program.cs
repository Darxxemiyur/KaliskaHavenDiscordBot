using DisCatSharp.Entities;
using DisCatSharp.EventArgs;

using KaliskaHaven.DiscordClient;
using KaliskaHaven.DiscordClient.SessionChannels;

using Name.Bayfaderix.Darxxemiyur.Common;
using Name.Bayfaderix.Darxxemiyur.Common.Async;
using Name.Bayfaderix.Darxxemiyur.Common.Extensions;

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

			await MyTaskExtensions.RunOnScheduler(() => Task.WhenAll(_services.RunRunnable(), RunTest()), scheduler: await sc.MyTaskSchedulerPromise);
		}

		private async Task RunTest()
		{
			var kali = await _services.GetKaliskaBot();

			var ch = new BareMessageChannel(kali, 1083244585410637885, null);

			
		}

		private static async Task Main(string[] args)
		{
			var prog = new Program();
			await prog.RunRunnable();
		}
	}
}