using DisCatSharp;
using DisCatSharp.Common.Utilities;
using DisCatSharp.Entities;
using DisCatSharp.EventArgs;

using Name.Bayfaderix.Darxxemiyur.Common;
using Name.Bayfaderix.Darxxemiyur.Common.Extensions;

using System.Reflection;

namespace KaliskaHaven.DiscordClient
{

	public sealed class KaliskaBot : IAsyncRunnable, IKaliskaBot
	{
		private enum StartupSteps
		{
			Initialize,
			SetUp,
			Ready,
		}
		private DisCatSharp.DiscordClient? _client;
		private StartupSteps _steps;
		private readonly IMyServices _service;
		private readonly AsyncLocker _lock;

		public Task<DisCatSharp.DiscordClient> GetClient() => _client == null ? Task.FromException<DisCatSharp.DiscordClient>(new NullReferenceException(nameof(_client))) : Task.FromResult(_client);

		public KaliskaBot(IMyServices service)
		{
			_service = service;
			_steps = StartupSteps.Initialize;
			_lock = new();
		}

		/// <summary>
		/// Sets up local
		/// </summary>
		/// <returns></returns>
		public async Task Initialize()
		{
			ThrowIfNotReady(StartupSteps.Initialize);

			_steps = StartupSteps.SetUp;
		}

		/// <summary>
		/// Sets up networking
		/// </summary>
		/// <returns></returns>
		public async Task SetUp()
		{
			ThrowIfNotReady(StartupSteps.SetUp);

			var config = await _service.ParseConfiguration();

			_client = new DisCatSharp.DiscordClient(config?.Discord?.GetConfig);

			_eventRouter = new(_client.GetType().GetEvents().Length);
			var versionRaw = Assembly.GetExecutingAssembly().GetName().Version;
			var version = $"Running v{{{versionRaw}}}";
			var activity = new DiscordActivity(version, ActivityType.Playing);

			await _client.ConnectAsync(activity);
			await _client.InitializeAsync();

			_steps = StartupSteps.Ready;
		}

		/// <summary>
		/// Runs bot
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public async Task RunRunnable(CancellationToken token = default)
		{
			ThrowIfNotReady();

			await Task.Delay(-1, token);
		}

		private void ThrowIfNotReady(StartupSteps step = StartupSteps.Ready)
		{
			if (_steps != step)
				throw new Exception();
		}

		/// <summary>
		/// Event router
		/// </summary>
		private Dictionary<Type, WeakReference<object>>? _eventRouter;

		public Task<DCEventRouter<DisCatSharp.DiscordClient, TEvent>> GetEventRouter<TEvent>() where TEvent : DiscordEventArgs => GetEventRouter<TEvent>(x => true);

		public Task GetInteractivity() => throw new NotImplementedException();

		public Task GetHandling() => throw new NotImplementedException();

		public Task<IMyServices> GetServices() => Task.FromResult(_service);

		public async Task<DCEventRouter<DisCatSharp.DiscordClient, TEvent>> GetEventRouter<TEvent>(Func<EventInfo, bool> selector) where TEvent : DiscordEventArgs
		{
			ThrowIfNotReady();

			await using var __ = await _lock.BlockAsyncLock();

			var etype = typeof(TEvent);
			if (_eventRouter?.TryGetValue(etype, out var @ref) == true)
				if (@ref.TryGetTarget(out var tgt))
					return (DCEventRouter<DisCatSharp.DiscordClient, TEvent>)tgt;
				else
					_eventRouter.Remove(etype);

			var events = _client.GetType().GetEvents();
			var nevent = events.First(x => x.EventHandlerType == typeof(AsyncEventHandler<DisCatSharp.DiscordClient, TEvent>) && selector(x));

			if (nevent == null)
				throw new ArgumentException(nameof(nevent));

			var router = new DCEventRouter<DisCatSharp.DiscordClient, TEvent>(_client, nevent.Name);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
			//The reference to _eventRouter is NOT null.
			_eventRouter.Add(etype, new(router));
#pragma warning restore CS8602 // Dereference of a possibly null reference.

			_ = MyTaskExtensions.RunOnScheduler(router.RunRunnable);
			return router;
		}
	}
}