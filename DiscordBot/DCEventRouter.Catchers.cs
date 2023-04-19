using DisCatSharp;
using DisCatSharp.Common.Utilities;
using DisCatSharp.EventArgs;

using Name.Bayfaderix.Darxxemiyur.Collections;
using Name.Bayfaderix.Darxxemiyur.Extensions;
using Name.Bayfaderix.Darxxemiyur.Tasks;

using System.Reflection;

namespace KaliskaHaven.DiscordClient
{
	public sealed partial class DCEventRouter<TClient, TEvent> where TClient : BaseDiscordClient where TEvent : DiscordEventArgs
	{
		private interface IEventCatcher
		{
			Task ReEnqueue(IEnumerable<TEvent> events);

			Task DiscardCatcher();
		}

		private sealed class MyEventBusCatcher : IEventCatcher, IAsyncRunnable
		{
			private readonly FIFOPTACollection<TEvent> _pouch;
			private readonly EventBus<TEvent> _bus;
			private readonly CancellationTokenSource _source;

			public MyEventBusCatcher(EventBus<TEvent> bus, FIFOPTACollection<TEvent> pouch)
			{
				_pouch = pouch;
				_bus = bus;
				_source = new();
			}

			public async Task DiscardCatcher()
			{
				await _bus.Stop();
				_source.Cancel();
			}

			public Task ReEnqueue(IEnumerable<TEvent> events) => _pouch.PlaceFirst(events);

			public Task ReturnEvent(TClient _, TEvent @event) => _bus.ReturnAndIgnoreItem(@event);

			public async Task RunRunnable(CancellationToken token = default)
			{
				using var linked = CancellationTokenSource.CreateLinkedTokenSource(token, _source.Token);
				while (!linked.Token.IsCancellationRequested)
				{
					var item = await _bus.GetItem();
					await _pouch.PlaceLast(item);
				}
			}
		}

		private sealed class MyEventCatcher : IEventCatcher, IDisposable, IAsyncDisposable
		{
			private readonly EventInfo _event;
			private readonly FIFOPTACollection<TEvent> _pouch;
			private readonly TClient _client;
			private readonly TaskScheduler _tScheduler;
			private bool _disposedValue;

			private Task Handler(TClient _, TEvent args) => _pouch.PlaceLast(args);

			public Task ReEnqueue(IEnumerable<TEvent> events) => _pouch.PlaceFirst(events);

			public MyEventCatcher(TClient client, EventInfo @event, FIFOPTACollection<TEvent> pouch, TaskScheduler scheduler)
			{
				_client = client;
				_event = @event;
				_pouch = pouch;
				_tScheduler = scheduler;
				_event.AddEventHandler(client, (AsyncEventHandler<TClient, TEvent>)Handler);
			}

			private void Dispose(bool disposing)
			{
				if (_disposedValue)
					return;

				if (disposing)
					_event.RemoveEventHandler(_client, (AsyncEventHandler<TClient, TEvent>)Handler);

				_disposedValue = true;
			}

			~MyEventCatcher() => Dispose(disposing: false);

			public void Dispose()
			{
				Dispose(disposing: true);
				GC.SuppressFinalize(this);
			}

			public async ValueTask DisposeAsync()
			{
				await MyTaskExtensions.RunOnScheduler(() => Dispose(disposing: true), default, _tScheduler);
				GC.SuppressFinalize(this);
			}

			public async Task DiscardCatcher() => await DisposeAsync();
		}
	}
}
