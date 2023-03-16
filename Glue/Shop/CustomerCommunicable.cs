using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Shop;

/// <summary>
/// Represents Customer communication channel.
/// </summary>
public abstract class CustomerCommunicable : IMessageCommunicable
{
	public abstract CommunicableCapabilities Capabilities {
		get;
	}

	public abstract Task<CommunicableCapabilities> CapabilitiesAsync {
		get;
	}

	public abstract TellResult TellInternal(TellMessage message);

	public abstract Task<TellResult> TellInternalAsync(TellMessage message);

	public abstract IEnumerable<TellResult> TellInternalProcedurally(TellMessage message);

	public abstract IAsyncEnumerable<TellResult> TellInternalProcedurallyAsync(TellMessage message);
}
