using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Shop;

public static class CustomerCommExts
{
	public static bool IsOfType<T>(this ITellResult? result) => result?.Result?.GetType() == typeof(T);
}