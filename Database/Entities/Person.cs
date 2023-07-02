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

	public IAcquirable<ICollection<IGroup>> Groups => throw new NotImplementedException();
	public IIdentity? Identity => throw new NotImplementedException();

	public IUser? Identifyable => this;

	public Type Type {
		get;
	} = typeof(Person);

	public bool Equals<TId>(IIdentifiable<TId> to) => to is Person si && si.ID == ID;
}
