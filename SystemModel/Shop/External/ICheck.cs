using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Shop.External;

public interface ICheck
{
	IAsyncEnumerable<IIdentifiable<object>> Identifyables {
		get;
	}

	Task AddToCheck(IIdentifiable<object> identifyable);

	Task RemoveFromCheck(IIdentifiable<object> identifyable);
}