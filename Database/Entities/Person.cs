using KaliskaHaven.Database.Economy;
using KaliskaHaven.SocialModel;

using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Database.Entities;

public sealed class Person : IUser
{
	public long ID {
		get; set;
	}

	public Wallet? Wallet {
		get; set;
	}

	public ulong DiscordId {
		get; set;
	}

	public IAsyncEnumerable<IIdentifiable<IGroup>> Groups {
		get;
	}

	public IIdentity? Identity => throw new NotImplementedException();

	public IUser? Identifyable => this;

	public Type Type {
		get;
	} = typeof(Person);

	public IAsyncEnumerable<Permission> Permissions {
		get;
	}

	public bool Equals<TId>(IIdentifiable<TId> to) => to is Person si && si.ID == ID;
}
