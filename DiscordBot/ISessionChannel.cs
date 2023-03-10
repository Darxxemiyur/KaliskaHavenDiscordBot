namespace KaliskaHaven.DiscordClient
{
	/// <summary>
	/// Discord Session Channel. Exposes simple, easy to use way to send and receive text and files
	/// in/from discord. Behavior is not constrained. There are no guarantees in methods working the
	/// same way. Beware.
	/// </summary>
	public interface ISessionChannel
	{
		/// <summary>
		/// True if this Session Channel is usable. False otherwise.
		/// </summary>
		bool IsUsable {
			get;
		}

		/// <summary>
		/// Attempts to transform current session channel of one type to another. Always returns a
		/// new instance of the passed type.
		/// </summary>
		/// <typeparam name="TNewSessionChannel">The new session channel instance.</typeparam>
		/// <param name="channel"></param>
		/// <returns>True if transformed, false otherwise.</returns>
		bool TryTransformAs<TNewSessionChannel>(out TNewSessionChannel channel) where TNewSessionChannel : IServiceProvider;

		/// <summary>
		/// Checks if current session channel can be transformed into another session channel type.
		/// </summary>
		/// <typeparam name="TNewSessionChannel"></typeparam>
		/// <param name="channel"></param>
		/// <returns>True if can be transformed, false otherwise.</returns>
		bool CanTransformAs<TNewSessionChannel>() where TNewSessionChannel : IServiceProvider;

		/// <summary>
		/// Attempts to transform current session channel of one type to another. May throw an
		/// exception. Always returns a new instance of the passed type.
		/// </summary>
		/// <typeparam name="TNewSessionChannel"></typeparam>
		/// <param name="channel"></param>
		/// <returns>The new Session Channel instance</returns>
		TNewSessionChannel TransformAs<TNewSessionChannel>() where TNewSessionChannel : IServiceProvider;

		/// <summary>
		/// Returns an EventBus which is configured to events related to the session channel.
		/// </summary>
		/// <typeparam name="TEvent"></typeparam>
		/// <returns></returns>
		Task<EventBus<TEvent>> GetSessionRelatedEvents<TEvent>() where TEvent : class;

		/// <summary>
		/// Tells instance to send a universal message with some concrete content.
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		Task SendMessage(UniversalMessageBuilder message);

		/// <summary>
		/// Tells instance to deactivate all components on the message.
		/// Note: "The" is up to the instance.
		/// </summary>
		/// <returns></returns>
		Task DeactivateAllComponents();

		/// <summary>
		/// Tells instance to remove the message.
		/// Note: "The" is up to the instance.
		/// </summary>
		/// <returns></returns>
		Task RemoveMessage();

		/// <summary>
		/// Communicate Session Channel instance an object.
		/// </summary>
		/// <returns>True if instance has accepted and understood the message. False otherwise.</returns>
		Task<TellResult> TellInternal(TellMessage message);

		/// <summary>
		/// Communicate Session Channel instance an object.
		/// </summary>
		/// <returns>Procedural communication channel that is up to the session channel instance to implement.</returns>
		IAsyncEnumerable<TellResult> TellInternalProcedurally(TellMessage message);
	}
	/// <summary>
	/// A non nullable value structure. Do not mistake for ITellMessage. They are not the same.
	/// </summary>
	public record struct TellMessage(ITellMessage? OriginalMessage) : ITellMessage
	{
		public bool IsNull => OriginalMessage == null;
		public string? Note => OriginalMessage?.Note;
		public object? Message => OriginalMessage?.Message;
	}
	/// <summary>
	/// A non nullable value structure. Do not mistake for ITellResult. They are not the same.
	/// </summary>
	public record struct TellResult(ITellResult? OriginalResult) : ITellResult
	{
		public bool IsNull => OriginalResult == null;
		public int Code => OriginalResult?.Code ?? -1;
		public object? Payload => OriginalResult?.Payload;
		public string? Note => OriginalResult?.Note;
	}
	/// <summary>
	/// TellInternally's result interface.
	/// </summary>
	public interface ITellResult
	{
		int Code {
			get;
		}
		object? Payload {
			get;
		}
		string? Note {
			get;
		}
	}
	/// <summary>
	/// TellInternally's message interface
	/// </summary>
	public interface ITellMessage
	{
		object? Message {
			get;
		}
		string? Note {
			get;
		}
	}
}