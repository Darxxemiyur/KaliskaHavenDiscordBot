using Name.Bayfaderix.Darxxemiyur.Node.Network;

using System.Runtime.Serialization;

namespace KaliskaHaven.Glue;

/// <summary>
/// Glued network interface.
/// </summary>
public interface IGluedNetwork : INodeNetwork
{
	/// <summary>
	/// Glued network cache.
	/// </summary>
	NodeNetworkCache Cache {
		get;
	}

	/// <summary>
	/// Glued network persistant data.
	/// </summary>
	NodeNetworkPersistant Persistant {
		get;
	}

	/// <summary>
	/// Glued network services.
	/// </summary>
	NodeNetworkServices Services {
		get;
	}
}

/// <summary>
/// Local cached variables. Do not need any serialization because they are simply cached.
/// </summary>
public abstract class NodeNetworkCache
{
}

/// <summary>
/// Persistant (serializable) data
/// </summary>
[Serializable]
public abstract class NodeNetworkPersistant : ISerializable
{
	public abstract void GetObjectData(SerializationInfo info, StreamingContext context);

	protected NodeNetworkPersistant(SerializationInfo info, StreamingContext context)
	{
	}
}

/// <summary>
/// System services that are always present.
/// </summary>
public abstract class NodeNetworkServices
{
}
