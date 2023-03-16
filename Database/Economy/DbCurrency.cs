using KaliskaHaven.Economy;

using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Database.Economy
{
	public sealed class DbCurrency : Currency, IIdentifiable<Currency>
	{
		public IIdentity? Identity => throw new NotImplementedException();
		public Currency? Identifyable => this;
		public ulong ID {
			get; set;
		}
		public Type Type {
			get;
		} = typeof(DbCurrency);
		public DbCurrency()
		{
		}

		public DbCurrency(Currency currency)
		{
			if (currency is DbCurrency db)
				ID = db.ID;
			Quantity = currency.Quantity;
			CurrencyType = currency.CurrencyType;
		}

		public bool Equals<TId>(IIdentifiable<TId> to) => throw new NotImplementedException();
	}
}
