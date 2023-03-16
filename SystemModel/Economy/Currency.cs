namespace KaliskaHaven.Economy;

/// <summary>
/// Currency type
/// </summary>
public enum CurrencyType
{
	Money
}

/// <summary>
/// Currency
/// </summary>
public sealed class Currency
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