using Name.Bayfaderix.Darxxemiyur.Collections;
using Name.Bayfaderix.Darxxemiyur.General;
using Name.Bayfaderix.Darxxemiyur.Tasks;

namespace KaliskaHaven.DiscordClient
{
	/// <summary>
	/// Actions to take when marked as
	/// </summary>
	public enum EventBusInfo
	{
		Pass,
		Remove,
	}

	internal interface IEventBusSource<TEvent> where TEvent : class
	{
		Task ReEnqueue(IEnumerable<TEvent> events);

		Task OutsourceReEnqueue(FIFOFBACollection<TEvent> pouch);
	}

	/// <summary>
	/// Link between orderer and passer.
	/// </summary>
	/// <typeparam name="TEvent"></typeparam>
	public sealed class EventBus<TEvent> : IDisposable, IAsyncDisposable where TEvent : class
	{
		private readonly AsyncLocker _lock;
		private readonly FIFOFBACollection<TEvent> _pouch;
		private readonly CancellationToken _token;
		private readonly Func<TEvent, Task<bool>> _predictator;
		private readonly IEventBusSource<TEvent> _back;
		private readonly LinkedList<WeakReference<TEvent>> _ignore;
		private readonly ExternalOnFinalization _onFinalize;
		private EventBusInfo _flags;
		private bool _disposedValue;

		/// <summary>
		/// Link between orderer and passer.
		/// </summary>
		/// <param name="predictator">The predicator</param>
		/// <param name="back">The callback interface</param>
		/// <param name="eventStorage">Long living event storage that EventBus temporarily uses as a channel to get events from. After EventBus is finalized, the storage gets re-uploaded into the router.</param>
		/// <param name="del"></param>
		/// <param name="token"></param>
		internal EventBus(Func<TEvent, Task<bool>> predictator, IEventBusSource<TEvent> back, FIFOFBACollection<TEvent> eventStorage, ExternalOnFinalization del, CancellationToken token = default)
		{
			_onFinalize = del;
			_lock = new();
			_pouch = eventStorage;
			_ignore = new();
			_token = token;
			_predictator = predictator;
			_back = back;
			_flags = EventBusInfo.Pass;
		}

		/// <summary>
		/// Accept item to the storage.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public async Task PassItem(TEvent item)
		{
			await using var __ = await _lock.ScopeAsyncLock();
			if (_flags == EventBusInfo.Pass)
				await _pouch.Handle(item);
			else
				await _back.ReEnqueue(new[] { item });
		}

		/// <summary>
		/// Returns item back to the event router and ignores it.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public Task ReturnAndIgnoreItem(TEvent item) => ReturnAndIgnoreItem(new[] { item });

		/// <summary>
		/// Returns items back to the event router and ignores them.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public async Task ReturnAndIgnoreItem(IEnumerable<TEvent> items)
		{
			await using var __ = await _lock.ScopeAsyncLock();
			foreach (var item in items)
				_ignore.AddLast(new WeakReference<TEvent>(item));
			await _back.ReEnqueue(items);
		}

		/// <summary>
		/// Does our item fit?
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public async Task<bool> CompareItem(TEvent? item)
		{
			await using var __ = await _lock.ScopeAsyncLock();

			var isNotIgnored = true;
			var next = _ignore.First;

			while (next != null && item != null)
			{
				var curr = next;
				next = next.Next;
				if (curr.Value.TryGetTarget(out var ignore))
					isNotIgnored &= ignore != item;
				else
					_ignore.Remove(curr);
			}

			return isNotIgnored && item != null && _flags == EventBusInfo.Pass && await _predictator(item);
		}

		public async Task<EventBusInfo> GetInfo()
		{
			await using var __ = await _lock.ScopeAsyncLock();

			return _flags;
		}

		/// <summary>
		/// Get's placed inside events.
		/// </summary>
		/// <returns></returns>
		public async Task<TEvent> GetItem(CancellationToken token = default)
		{
			using var tokenS = CancellationTokenSource.CreateLinkedTokenSource(_token, token);
			return await _pouch.GetData(tokenS.Token);
		}

		public Task<bool> HasAny() => _pouch.HasAny();

		/// <summary>
		/// Finishes the bus polling.
		/// </summary>
		/// <returns></returns>
		public async Task Stop()
		{
			await using var __ = await _lock.ScopeAsyncLock();

			if (_disposedValue || _flags == EventBusInfo.Remove)
				return;

			if (_flags == EventBusInfo.Pass)
				_flags = EventBusInfo.Remove;

			_onFinalize?.Call();
		}

		public async ValueTask DisposeAsync() => await Stop();

		private void Dispose(bool disposing)
		{
			if (_disposedValue)
				return;

			if (_flags != EventBusInfo.Remove)
			{
				_onFinalize?.Call();
			}

			_disposedValue = true;
		}

		~EventBus() => Dispose(disposing: false);

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
