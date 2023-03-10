using DisCatSharp;
using DisCatSharp.Common.Utilities;
using DisCatSharp.EventArgs;

using Name.Bayfaderix.Darxxemiyur.Common;
using Name.Bayfaderix.Darxxemiyur.Common.Extensions;

namespace KaliskaHaven.DiscordClient
{
	public sealed partial class DCEventRouter<TClient, TEvent> : IAsyncRunnable where TClient : BaseDiscordClient where TEvent : DiscordEventArgs
	{
		private readonly Runner _runner;

		/// <summary>
		/// We dont care about the exact type of the catcher, we just care that it needs to be
		/// disposed of when we are done with event router.
		/// </summary>
		private readonly IEventCatcher _catcher;

		private readonly TaskScheduler _tScheduler;
		private readonly FIFOFBACollection<IAsyncRunnable> _pendingChildren;

		/// <summary>
		/// </summary>
		/// <param name="client"></param>
		/// <param name="eventName"></param>
		/// <exception cref="ArgumentException"></exception>
		public DCEventRouter(TClient client, string eventName)
		{
			var @event = typeof(TClient).GetEvent(eventName) ??
				throw new ArgumentException($"The {nameof(eventName)} provided (which is {eventName}) isn't a valid member of {{{typeof(TClient).FullName}}}.");

			if (@event.EventHandlerType != typeof(AsyncEventHandler<TClient, TEvent>))
				throw new ArgumentException($"The {nameof(eventName)} provided (which is {{{eventName}}}) isn't a valid event of the {{{typeof(AsyncEventHandler<TClient, TEvent>).FullName}}} type.");

			var pouch = new FIFOPTACollection<TEvent>();
			_tScheduler = MyTaskExtensions.GetScheduler();
			_pendingChildren = new(true);
			_catcher = new MyEventCatcher(client, @event, pouch, _tScheduler);
			_runner = new(client, _catcher, pouch, _tScheduler, _pendingChildren);
		}

		public async Task<DCEventRouter<TClient, TEvent>> CreateSubRouter(Func<TEvent, bool> predictator, CancellationToken token = default)
		{
			var bus = await PlaceRequest(predictator, token);
			var toRun = new LinkedList<Func<Task>>();
			var router = new DCEventRouter<TClient, TEvent>(this, bus, toRun);
			await MyTaskExtensions.RunOnScheduler(() => Task.WhenAll(toRun.Select(x => x())), default, _tScheduler);
			return router;
		}

		private DCEventRouter(DCEventRouter<TClient, TEvent> parent, EventBus<TEvent> bus, LinkedList<Func<Task>> toRun)
		{
			_pendingChildren = new(true);
			var pouch = new FIFOPTACollection<TEvent>();
			_tScheduler = parent._tScheduler;
			var catcher = new MyEventBusCatcher(bus, pouch, parent);
			_runner = new(parent._runner.KeepAliveReference, catcher, pouch, _tScheduler, _pendingChildren);
			_catcher = catcher;
			_runner.OnNonFiltered += catcher.ReturnEvent;
			toRun.AddLast(() => _pendingChildren.Handle(catcher));
			toRun.AddLast(() => parent._pendingChildren.Handle(this));
		}

		~DCEventRouter() => MyTaskExtensions.RunOnScheduler(() => _runner.Stop(), default, _tScheduler);

		public Task<EventBus<TEvent>> PlaceRequest(CancellationToken token = default) => PlaceRequest(x => true, token);

		public Task<EventBus<TEvent>> PlaceRequest(Func<TEvent, bool> predictator, CancellationToken token = default) => PlaceRequest(x => Task.FromResult(predictator(x)), token);

		/// <summary>
		/// Caller manages the lifetime of the EventBus. The EventRouter uses weak reference to
		/// access the EventBus. Upon being GC collected, the EventBuss will place its event bag
		/// back into router.
		/// </summary>
		/// <param name="predictator"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public Task<EventBus<TEvent>> PlaceRequest(Func<TEvent, Task<bool>> predictator, CancellationToken token = default) => _runner.PlaceRequest(predictator, ReEqn, OnSyncDeathCallback, token);
		private Task ReEqn(IEnumerable<TEvent> queue) => _catcher.ReEnqueue(queue);

		private Task OnSyncDeathCallback(FIFOFBACollection<TEvent> pouchToRecover) => _runner.OnSyncDeathCallback(pouchToRecover);

		public Task RunRunnable(CancellationToken token = default) => ((IAsyncRunnable)_runner).RunRunnable(token);
	}
}