using KaliskaHaven.Database.Entities;
using KaliskaHaven.Economy;

using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Database.Economy
{
	public interface IDbWallet : IWallet, IIdentifiable<IWallet>
	{
		public long ID {
			get;
		}

		public ICollection<DbCurrency> DbCurrencies {
			get;
		}

		public Person? Owner {
			get;
		}

		/// <summary>
		/// Transfer 
		/// </summary>
		/// <param name="currency"></param>
		/// <returns>Transaction</returns>
		Task<IIdentifiable<ITransactionLog>> Transfer(IDbWallet receiver, Currency currency);
	}
}
