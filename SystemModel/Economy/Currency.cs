namespace KaliskaHaven.Economy
{
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
	}
}