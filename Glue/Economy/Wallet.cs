using KaliskaHaven.Database;
using KaliskaHaven.Database.Economy;
using KaliskaHaven.Database.Entities;
using KaliskaHaven.Economy;

using Name.Bayfaderix.Darxxemiyur.Async;
using Name.Bayfaderix.Darxxemiyur.Extensions;
using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Glue.Economy
{
	public sealed class Wallet : IDbWallet
	{
		private static readonly MySingleThreadSyncContext s_My = new(ThreadPriority.Lowest);

		public static async Task<Stream> GetWalletImage(Wallet wallet, string currencies, string username, string userUrl) => await MyTaskExtensions.RunOnScheduler(new Func<Task<Stream>>(async () => {
			using var hc = new HttpClient();
			using var iconImgH = await hc.GetAsync(userUrl);
			using var iconImg = await iconImgH.Content.ReadAsStreamAsync();

			using var memii = new MemoryStream();
			await iconImg.CopyToAsync(memii);
			memii.Seek(0, SeekOrigin.Begin);

			using var icoImg = await Image.LoadAsync(memii);
			using var img = new Image<Rgba32>(500, 300);

			await MyTaskExtensions.RunOnScheduler(() => {
				icoImg.Mutate(x => {
					var s = new Size(120, 120);
					x.Resize(s);
				});
				img.Mutate(x => {
					using var bg = new Image<Rgba32>(img.Size.Width, img.Size.Height);
					bg.Mutate(y => y.BackgroundColor(new Color(new Rgba32(255 / 2, 255 / 2, 255 / 2))));
					x.DrawImage(bg, new Point(0, 0), .5f);
					x.DrawImage(icoImg, new Point(img.Size.Width - 120, 0), 1f);
				});
			});

			var ms = new MemoryStream();
			await img.SaveAsPngAsync(ms);
			ms.Seek(0, SeekOrigin.Begin);
			return ms;
		}), default, await s_My.MyTaskSchedulerPromise);

		private readonly KaliskaDB _db;
		private readonly Database.Economy.Wallet _wallet;

		public Wallet(KaliskaDB db, Database.Economy.Wallet wallet)
		{
			_db = db;
			_wallet = wallet;
		}

		public long ID => _wallet.ID;

		public ICollection<DbCurrency> DbCurrencies => _wallet.DbCurrencies;

		public Person Owner => _wallet.Owner;

		public IIdentity? Identity => _wallet.Identity;

		public IWallet? Identifyable => _wallet.Identifyable;

		public Type Type => _wallet.Type;

		public async Task EnsureFullyLoaded()
		{
			var entry = _db.Entry(_wallet);
			await entry.Collection(x => x.DbCurrencies).LoadAsync();
			await entry.Reference(x => x.Owner).LoadAsync();
			await _db.SaveChangesAsync();
		}

		public async Task<IIdentifiable<ITransactionLog>> Deposit(Currency currency)
		{
			await this.EnsureFullyLoaded();
			var transaction = await _wallet.Deposit(currency);
			await _db.SaveChangesAsync();
			return transaction;
		}

		public bool Equals<TId>(IIdentifiable<TId> to) => to is IIdentifiable<IWallet> wa && wa is IDbWallet dw && dw.ID == ID;

		public async Task<Currency?> Get(CurrencyType currency)
		{
			await this.EnsureFullyLoaded();
			return await _wallet.Get(currency);
		}

		public async IAsyncEnumerable<Currency> GetAllCurrencies()
		{
			await this.EnsureFullyLoaded();
			await foreach (var curr in _wallet.GetAllCurrencies())
				yield return curr;
		}

		public async Task<IIdentifiable<ITransactionLog>> Withdraw(Currency currency)
		{
			await this.EnsureFullyLoaded();
			var transaction = await _wallet.Withdraw(currency);
			await _db.SaveChangesAsync();
			return transaction;
		}
	}
}
