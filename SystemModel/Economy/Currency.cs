namespace KaliskaHaven.Economy;

/// <summary>
/// Currency type
/// </summary>
public enum CurrencyType
{
	Kelpie_Cash,
}

/// <summary>
/// Currency
/// </summary>
public class Currency
{
	public CurrencyType CurrencyType {
		get; set;
	}

	public long Quantity {
		get => _quantity / Delim; set => _quantity = value * Delim;
	}

	private long _quantity;

	public const int Delim = 1;

	public Currency(CurrencyType type, long quantity) => (CurrencyType, Quantity) = (type, quantity);

	public static implicit operator float(Currency currency) => currency.Quantity / (float)Delim;

	public static implicit operator double(Currency currency) => currency.Quantity / (double)Delim;

	public static implicit operator long(Currency currency) => currency.Quantity / Delim;
}
