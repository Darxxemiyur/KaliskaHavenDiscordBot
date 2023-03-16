using KaliskaHaven.SocialModel;

using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Database.Entities;

public sealed class Person : IUser
{
	public ulong ID {
		get; set;
	}

	public IAsyncEnumerable<IIdentifiable<IGroup>> Groups {
		get;
	}

	public IIdentity? Identity {
		get;
	}

	public IUser? Identifyable {
		get;
	}

	public Type Type {
		get;
	} = typeof(Person);

	public bool Equals<TId>(IIdentifiable<TId> to) => to is Person si && si.ID == ID;
}