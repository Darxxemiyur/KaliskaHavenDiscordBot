using Name.Bayfaderix.Darxxemiyur.Collections;
using Name.Bayfaderix.Darxxemiyur.Extensions;
using Name.Bayfaderix.Darxxemiyur.General;
using Name.Bayfaderix.Darxxemiyur.Tasks;

namespace KaliskaHaven.Database
{
	internal sealed class DBCacheNode
	{
		public KaliskaDBBackend? DBBackend {
			get; set;
		}

		public WeakReference<KaliskaDB>? Facade {
			get; set;
		}

		public LinkedListNode<DBCacheNode>? Self {
			get; set;
		}

		public ExternalOnFinalization<DBCacheNode> Finalization {
			get;
		}
		internal DBCacheNode() => Finalization = new(this);
	}

	public sealed class DBCache : IAsyncRunnable
	{
		private readonly LinkedList<DBCacheNode> _cache = new();
		private readonly AsyncLocker _lock = new();
		private readonly FIFOFBACollection<DBCacheNode> _relay = new();

		public async Task RunRunnable(CancellationToken token = default)
		{
			while (!token.IsCancellationRequested)
			{
				var node = await _relay.GetData(token);
				_cache.Remove(node.Self.ThrowIfNull());
				await node.DBBackend.ThrowIfNull().DisposeAsync();

			}
		}

		public async Task<KaliskaDB> Create()
		{
			await using var __ = await _lock.ScopeAsyncLock();
			var node = new DBCacheNode();
			node.DBBackend = new KaliskaDBBackend();
			var db = new KaliskaDB(node);
			node.Facade = new(db);
			node.Self = _cache.AddLast(node);
			node.Finalization.Actions.AddLast((x) => MyTaskExtensions.RunOnScheduler(() => _relay.Handle(x)));

			return db;
		}
	}
}
