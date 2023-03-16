using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Glue.Shop;

public interface ICheck
{
	IAsyncEnumerable<IIdentifiable<object>> Identifyables {
		get;
	}

	Task AddToCheck(IIdentifiable<object> identifyable);

	Task RemoveFromCheck(IIdentifiable<object> identifyable);
}
