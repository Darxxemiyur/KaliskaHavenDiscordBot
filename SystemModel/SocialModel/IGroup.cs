using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.SocialModel;

public interface IGroup : IIdentifiable<IGroup>
{
	IAcquirable<ICollection<IUser>> Members {
		get;
	}
}
