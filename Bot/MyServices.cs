using Microsoft.Extensions.Configuration;

using Name.Bayfaderix.Darxxemiyur.Async;
using Name.Bayfaderix.Darxxemiyur.Extensions;
using Name.Bayfaderix.Darxxemiyur.Tasks;

using System.Reflection;

namespace KaliskaHaven.DiscordClient
{
	public class MyServices : IAsyncRunnable, IMyServices
	{
		private readonly KaliskaBot _kaliskaBot;

		public MyServices()
		{
			_kaliskaBot = new(this);
		}

		public Task<IKaliskaBot> GetKaliskaBot() => Task.FromResult((IKaliskaBot)_kaliskaBot);

		private const string ConfigFile = "bot.settings.json";

		public async Task<Config> ParseConfiguration()
		{
			var conf = await this.Load();
			await this.Update(conf);
			return await this.Validate(conf) ? conf : throw new ArgumentException();
		}

		private async Task<bool> Validate(Config? conf)
		{
			return !string.IsNullOrEmpty(conf?.Discord?.Token);
		}

		private async Task Update(Config? conf)
		{
		}

		private async Task<Config?> Load()
		{
			IConfiguration config = await MyTaskExtensions.RunOnScheduler(() => new ConfigurationBuilder().AddJsonFile(ConfigFile).AddEnvironmentVariables().Build());

			return await MyTaskExtensions.RunOnScheduler(() => config.Get<Config>());
		}

		public async Task Initialize()
		{
			var context = new MySingleThreadSyncContext();
			var tsc = await context.MyTaskSchedulerPromise;

			await MyTaskExtensions.RunOnScheduler(_kaliskaBot.Initialize, default, tsc);
			await MyTaskExtensions.RunOnScheduler(_kaliskaBot.SetUp, default, tsc);
		}

		public async Task RunRunnable(CancellationToken token = default)
		{
			await MyTaskExtensions.RunOnScheduler(_kaliskaBot.RunRunnable, token);
		}

		public async Task<IDbFactory<TDatabase>> GetDbFactory<TDatabase>() where TDatabase : class
		{
			var factory = await MyTaskExtensions.RunOnScheduler(() => {
				var dbType = typeof(TDatabase);
				var dbFactoryType = typeof(IDbFactory<TDatabase>);

				var asm = Assembly.GetAssembly(dbType);

				if (asm == null)
					throw new ArgumentException($"Assembly of {dbType} does not exist");

				var list = asm.ExportedTypes.Where(x => x.IsAssignableTo(dbFactoryType) && !x.IsAbstract && !x.IsInterface && x.GetConstructors().Any(x => x.IsPublic && !x.GetParameters().Any())).ToList();

				if (list.Count != 1)
					throw new ArgumentOutOfRangeException($"The amount of types to create isn't exactly 1! It was {list.Count}!" + (list.Count > 0 ? $" Found these types: \n{string.Join(Environment.NewLine, list.Select(x => $"<--<>-->{x.AssemblyQualifiedName}"))}" : ""));

				var factoryType = list.First();

				return Activator.CreateInstance(factoryType) as IDbFactory<TDatabase>;
			});

			await factory.PrepareFactory(this);

			return factory;
		}

		public interface IDbFactory<TDatabase> where TDatabase : class
		{
			Task PrepareFactory(MyServices myServices);
		}
	}
}
