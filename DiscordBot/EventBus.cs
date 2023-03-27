using Name.Bayfaderix.Darxxemiyur.Collections;
using Name.Bayfaderix.Darxxemiyur.Extensions;
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
		private readonly Func<IEnumerable<TEvent>, Task> _reEnqueue;
		private readonly Func<FIFOFBACollection<TEvent>, Task> _outSourcedReEnq;
		private readonly LinkedList<WeakReference<TEvent>> _ignore;
		private readonly ExternalOnFinalization _onFinalize;
		private EventBusInfo _flags;
		private bool _disposedValue;

		public EventBus(Func<TEvent, Task<bool>> predictator, Func<IEnumerable<TEvent>, Task> reEnqueue, Func<FIFOFBACollection<TEvent>, Task> outSourcedReEnq, ExternalOnFinalization del, CancellationToken token = default)
		{
			_onFinalize = del;
			_lock = new();
			_pouch = new();
			_ignore = new();
			_token = token;
			_predictator = predictator;
			_reEnqueue = reEnqueue;
			_outSourcedReEnq = outSourcedReEnq;
			_flags = EventBusInfo.Pass;
		}

		public async Task PassItem(TEvent item)
		{
			await using var _ = await _lock.BlockAsyncLock();
			if (_flags == EventBusInfo.Pass)
				await _pouch.Handle(item);
			else
				await _reEnqueue(new[] { item });
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
			await using var _ = await _lock.BlockAsyncLock();
			foreach (var item in items)
				_ignore.AddLast(new WeakReference<TEvent>(item));
			await _reEnqueue(items);
		}

		/// <summary>
		/// Does our item fit?
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public async Task<bool> CompareItem(TEvent? item)
		{
			await using var _ = await _lock.BlockAsyncLock();

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
			await using var _ = await _lock.BlockAsyncLock();

			return _flags;
		}

		/// <summary>
		/// Get's placed inside events.
		/// </summary>
		/// <returns></returns>
		public Task<TEvent> GetItem() => _pouch.GetData(_token);

		public Task<bool> HasAny() => _pouch.HasAny();

		/// <summary>
		/// Finishes the bus polling.
		/// </summary>
		/// <returns></returns>
		public async Task Stop()
		{
			await using var _ = await _lock.BlockAsyncLock();

			if (_disposedValue || _flags == EventBusInfo.Remove)
				return;

			if (_flags == EventBusInfo.Pass)
				_flags = EventBusInfo.Remove;

			_onFinalize?.Call();
			await _outSourcedReEnq(_pouch);
		}

		public async ValueTask DisposeAsync() => await Stop();

		private void Dispose(bool disposing)
		{
			if (_disposedValue)
				return;

			if (_flags != EventBusInfo.Remove)
			{
				_onFinalize?.Call();
				_ = MyTaskExtensions.RunOnScheduler(() => _outSourcedReEnq(_pouch));
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
