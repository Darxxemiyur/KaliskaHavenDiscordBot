using KaliskaHaven.SocialModel;

using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Database.Entities;

public sealed class Group : IGroup
{
	public ulong ID {
		get; set;
	}

	IAsyncEnumerable<IIdentifiable<IUser>> IGroup.Members => this.GetIdentifyablesAsync();

	private async IAsyncEnumerable<IIdentifiable<IUser>> GetIdentifyablesAsync()
	{
		foreach (var member in await Task.FromResult(Members))
			yield return member;
	}

	public LinkedList<Person> Members {
		get; set;
	}

	public IIdentity? Identity => throw new NotImplementedException();
	public IGroup? Identifyable => this;

	public Type Type {
		get;
	} = typeof(Group);

	public bool Equals<TId>(IIdentifiable<TId> to) => to is Group si && si.ID == ID;
}
