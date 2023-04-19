using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.SocialModel;

public interface IUser : IIdentifiable<IUser>
{
	IAsyncEnumerable<IIdentifiable<IGroup>> Groups {
		get;
	}
}
