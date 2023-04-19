namespace KaliskaHaven.Shop;

/// <summary>
/// Information about shop item options.
/// </summary>
public interface IOptionInfo
{
	Task<IConfigurableOption> ConstructConfigurableOption();
}
