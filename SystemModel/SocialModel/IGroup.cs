using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.SocialModel;

public interface IGroup : IIdentifiable<IGroup>
{
	IAsyncEnumerable<IIdentifiable<IUser>> Members {
		get;
	}
	IAsyncEnumerable<Permission> Permissions {
		get;
	}
}
