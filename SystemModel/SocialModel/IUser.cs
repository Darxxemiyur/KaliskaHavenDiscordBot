using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.SocialModel;

public interface IUser : IIdentifiable<IUser>
{
	IAcquirable<ICollection<IGroup>> Groups {
		get;
	}
}
