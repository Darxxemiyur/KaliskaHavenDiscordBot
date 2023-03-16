using KaliskaHaven.Database.Entities;
using KaliskaHaven.Economy;

using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Database.Economy
{
	public interface IDbWallet : IWallet, IIdentifiable<IWallet>
	{
		public ulong ID {
			get;
		}

		public ICollection<DbCurrency> DbCurrencies {
			get;
		}

		public Person Owner {
			get;
		}
	}
}
