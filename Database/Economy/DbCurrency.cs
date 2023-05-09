using KaliskaHaven.Economy;

using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Database.Economy
{
	public sealed class DbCurrency : Currency, IIdentifiable<Currency>
	{
		public IIdentity? Identity => throw new NotImplementedException();
		public Currency? Identifyable => this;

		public long ID {
			get; set;
		}

		public Type Type => typeof(DbCurrency);

		public DbCurrency(Currency currency) : this(currency.CurrencyType, currency.Quantity)
		{
		}

		public DbCurrency(CurrencyType currencyType, long quantity) : base(currencyType, quantity)
		{
		}

		//TODO: Make it possible to retrieve DbCurrency via some mechanism (like IMessageCommunicable or new one.)
		public DbCurrency(IIdentifiable<Currency> currency) : base(currency.Identifyable!.CurrencyType, currency.Identifyable!.Quantity)
		{
			if (currency is DbCurrency db)
				ID = db.ID;
		}

		public bool Equals<TId>(IIdentifiable<TId> to) => throw new NotImplementedException();
	}
}
