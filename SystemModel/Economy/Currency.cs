namespace KaliskaHaven.Economy;

/// <summary>
/// Currency type
/// </summary>
public enum CurrencyType
{
	Kelpie_Cash,
	Spring_Cash,
	Autumn_Cash,
	Summer_Cash,
	Winter_Cash
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
		get; set;
	}

	public const int Delim = 100;

	public static implicit operator float(Currency currency) => currency.Quantity / (float)Delim;

	public static implicit operator double(Currency currency) => currency.Quantity / (double)Delim;

	public static implicit operator long(Currency currency) => currency.Quantity / Delim;
}
