using DisCatSharp.Enums;

using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.DiscordClient.SessionChannels
{
	public class PersonSessionChannel : ISessionChannel
	{
		public bool IsUsable {
			get;
		}

		public CommunicableCapabilities Capabilities {
			get;
		}

		public Task<CommunicableCapabilities> CapabilitiesAsync {
			get;
		}

		private InteractionResponseType _type;

		public PersonSessionChannel()
		{
		}

		public bool CanTransformAs<TNewSessionChannel>() where TNewSessionChannel : IServiceProvider => throw new NotImplementedException();

		public async Task DeactivateAllComponents()
		{
		}

		public async Task<EventBus<TEvent>> GetSessionRelatedEvents<TEvent>() where TEvent : class
		{
			throw new NotImplementedException();
		}

		public async Task RemoveMessage()
		{
		}

		public Task SendMessage(UniversalMessageBuilder message)
		{
			throw new NotImplementedException();
		}

		public TNewSessionChannel TransformAs<TNewSessionChannel>() where TNewSessionChannel : IServiceProvider => throw new NotImplementedException();

		public bool TryTransformAs<TNewSessionChannel>(out TNewSessionChannel channel) where TNewSessionChannel : IServiceProvider => throw new NotImplementedException();

		public Task<TellResult> TellInternalAsync(TellMessage message) => throw new NotImplementedException();

		public TellResult TellInternal(TellMessage message) => throw new NotImplementedException();

		public IEnumerable<TellResult> TellInternalProcedurally(TellMessage message) => throw new NotImplementedException();

		public IAsyncEnumerable<TellResult> TellInternalProcedurallyAsync(TellMessage message) => throw new NotImplementedException();
	}
}
