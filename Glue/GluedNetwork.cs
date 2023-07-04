using Name.Bayfaderix.Darxxemiyur.Node.Network;

using System.Runtime.Serialization;

namespace KaliskaHaven.Glue;

/// <summary>
/// Glued network interface.
/// </summary>
public abstract class GluedNetwork : INodeNetwork
{
	/// <summary>
	/// Run sub network while also storing our own data in database.
	/// </summary>
	/// <param name="subNetwork"></param>
	/// <returns></returns>
	protected async Task<bool> RunSubNetwork(GluedNetwork subNetwork)
	{
		var archive = await this.ArchiveNetwork();
		var boo = false;
		try
		{
			await ((INodeNetwork)subNetwork).RunNetwork();
		}
		finally
		{
			boo = await this.UnArchive(archive);
		}
		return boo;
	}

	public abstract Task<GluedNetworkTransferPayload> ArchiveNetwork();
	public abstract Task<bool> IsArchived();
	public abstract Task<bool> Commit();
	public abstract Task<bool> UnArchive(GluedNetworkTransferPayload payload);
	public abstract StepInfo GetStartingInstruction();
}

/// <summary>
/// Class that contains <see cref="GluedNetwork"/> data to serialize and restore it.
/// </summary>
[Serializable]
public sealed class GluedNetworkTransferPayload : ISerializable
{
	public void GetObjectData(SerializationInfo info, StreamingContext context) => throw new NotImplementedException();

	protected GluedNetworkTransferPayload(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
		throw new NotImplementedException();
	}
}