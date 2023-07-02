using Name.Bayfaderix.Darxxemiyur.Node.Network;

using System.Runtime.Serialization;

namespace KaliskaHaven.Distributed;

/// <summary>
/// Class of node result to either allow swapping of worker or keep it going on the same worker.
/// </summary>
public enum DNNNodeClass
{
	Free,
	Keep
}

[Serializable]
public abstract class DistributedNodeNetwork : IGeneralNodeNetwork, ISerializable
{
	protected DistributedNodeNetwork()
	{
	}

	protected DistributedNodeNetwork(SerializationInfo info, StreamingContext context)
	{
	}

	public abstract void GetObjectData(SerializationInfo info, StreamingContext context);

	public abstract Task<IGeneralStepInfo?> GetStartStepInfo(IGeneralStepInfo? payload);

	public abstract Task<IGeneralStepInfo?> GetStartStepInfo(object? payload = null);
}

[Serializable]
public abstract class DistributedNodeNetworkNode : IGeneralStepInfo, ISerializable
{
	public abstract DNNNodeClass Class {
		get;
	}

	protected DistributedNodeNetworkNode()
	{
	}

	protected DistributedNodeNetworkNode(SerializationInfo info, StreamingContext context)
	{
	}

	public abstract IGeneralNodeNetwork? ParentNetwork {
		get;
	}

	public abstract void GetObjectData(SerializationInfo info, StreamingContext context);

	public abstract Task<IGeneralStepInfo?> Run(IGeneralStepInfo? previous = null, CancellationToken token = default);
}
