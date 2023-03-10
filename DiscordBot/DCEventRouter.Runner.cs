using DisCatSharp;
using DisCatSharp.Common.Utilities;
using DisCatSharp.EventArgs;

using Name.Bayfaderix.Darxxemiyur;
using Name.Bayfaderix.Darxxemiyur.Common;
using Name.Bayfaderix.Darxxemiyur.Common.Extensions;

namespace KaliskaHaven.DiscordClient
{
	public sealed partial class DCEventRouter<TClient, TEvent> where TClient : BaseDiscordClient where TEvent : DiscordEventArgs
	{
		// TODO: THE ROUTERS MUST BE GC COLLECTABLE. AND THEIR DESCENDANTS TOO.
		private sealed class Runner : IAsyncRunnable
		{
			private readonly IEventCatcher _catcher;

			public TClient KeepAliveReference {
				get;
			}

			private readonly FIFOFBACollection<IAsyncRunnable> _pendingChildren;
			private readonly FIFOPTACollection<TEvent> _pouch;
			private readonly LinkedList<WeakReference<EventBus<TEvent>>> _eventReceivers;
			private readonly AsyncLocker _lock;

			[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3264:Events should be invoked", Justification = "The event DOES get invoked.")]
			public event AsyncEventHandler<TClient, TEvent>? OnNonFiltered;

			private readonly TaskScheduler _tScheduler;
			private readonly CancellationTokenSource _graceCancelSource = new();

			public Runner(TClient client, IEventCatcher catcher, FIFOPTACollection<TEvent> pouch, TaskScheduler scheduler, FIFOFBACollection<IAsyncRunnable> childrenPouch)
			{
				_pouch = pouch;
				_tScheduler = scheduler;
				_pendingChildren = childrenPouch;
				_catcher = catcher;
				KeepAliveReference = client;
				_eventReceivers = new();
				_lock = new();
			}

			public async Task Stop()
			{
				await _catcher.DiscardCatcher();
				_graceCancelSource.Cancel();
			}

			private async Task EventLoop(CancellationToken token = default)
			{
				await foreach (var anEvent in _pouch.WithCancellation(token))
					if (token.IsCancellationRequested)
						await Task.FromCanceled(token);
					else if (!await PassToReceivers(anEvent))
						await PassToStatic(anEvent);
			}

			public async Task RunRunnable(CancellationToken token = default)
			{
				Task? eventLoop = null;
				Task? childrenStart = null;
				using var tokS = CancellationTokenSource.CreateLinkedTokenSource(_graceCancelSource.Token, token);
				while (!tokS.Token.IsCancellationRequested)
				{
					eventLoop ??= EventLoop(tokS.Token);
					childrenStart ??= ChildrenStart(tokS.Token);

					var completed = await Task.WhenAny(eventLoop, childrenStart);

					if (tokS.Token.IsCancellationRequested)
					{
						await Task.WhenAll(eventLoop, childrenStart);
						break;
					}

					if (eventLoop == completed)
						eventLoop = null;
					if (childrenStart == completed)
						childrenStart = null;
				}
			}

			private async Task PassToStatic(TEvent anEvent)
			{
				var receivers = OnNonFiltered?.GetInvocationList() ?? Enumerable.Empty<Delegate>();

				foreach (var place in receivers.OfType<AsyncEventHandler<TClient, TEvent>>())
				{
					await place.Invoke(KeepAliveReference, anEvent);
					if (anEvent.Handled)
						break;
				}
			}

			public async Task OnSyncDeathCallback(FIFOFBACollection<TEvent> pouchToRecover)
			{
				await _catcher.ReEnqueue(await pouchToRecover.ToEnumerableAsync());
				await pouchToRecover.Cancel();
			}

			private async Task ChildrenStart(CancellationToken token = default)
			{
				using var tc = CancellationTokenSource.CreateLinkedTokenSource(_graceCancelSource.Token, token);
				var child = await _pendingChildren.GetData(tc.Token);
				_ = MyTaskExtensions.RunOnScheduler(child, token, _tScheduler);
			}

			private async Task<bool> PassToReceivers(TEvent anEvent)
			{
				await using var _ = await _lock.BlockAsyncLock();

				var isPassed = false;
				var receiverNode = _eventReceivers.First;
				while (receiverNode != null)
				{
					var currNode = receiverNode;
					receiverNode = receiverNode.Next;
					if (currNode.Value.TryGetTarget(out var receiver))
					{
						if (await receiver.GetInfo() == EventBusInfo.Pass && await receiver.CompareItem(anEvent) && !isPassed && (isPassed = true))
							await receiver.PassItem(anEvent);

						if (await receiver.GetInfo() == EventBusInfo.Remove)
							_eventReceivers.Remove(currNode);
					}
					else
					{
						_eventReceivers.Remove(currNode);
					}
				}

				return isPassed;
			}

			public async Task<EventBus<TEvent>> PlaceRequest(Func<TEvent, Task<bool>> predictator, Func<IEnumerable<TEvent>, Task> reEnq, Func<FIFOFBACollection<TEvent>, Task> onDeath, CancellationToken token = default)
			{
				await using var _ = await _lock.BlockAsyncLock();
				var del = new ExternalOnFinalization();
				var thingy = new EventBus<TEvent>(predictator, reEnq, onDeath, del, token);
				var node = _eventReceivers.AddLast(new WeakReference<EventBus<TEvent>>(thingy));
				del.Delegate = () => _eventReceivers.Remove(node);
				return thingy;
			}
		}
	}
}