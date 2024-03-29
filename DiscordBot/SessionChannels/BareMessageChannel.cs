﻿using DisCatSharp.EventArgs;
using DisCatSharp.Exceptions;

using Name.Bayfaderix.Darxxemiyur.General;
using Name.Bayfaderix.Darxxemiyur.Tasks;

namespace KaliskaHaven.DiscordClient.SessionChannels;

public class BareMessageChannel : ISessionChannel
{
	private readonly IKaliskaBot _kaliska;
	private readonly ulong _channelId;
	private ulong? _messageId;
	private readonly AsyncLocker _lock;

	public BareMessageChannel(IKaliskaBot kaliska, ulong channelId, ulong? messageId)
	{
		_lock = new();
		_kaliska = kaliska;
		_channelId = channelId;
		_messageId = messageId;
	}

	public bool IsUsable {
		get; private set;
	} = true;

	public CommunicableCapabilities Capabilities {
		get;
	}

	public Task<CommunicableCapabilities> CapabilitiesAsync {
		get;
	}

	public IMessageCommunicable Comms {
		get;
	}

	public async Task DeactivateAllComponents()
	{
		Task sendJob;
		try
		{
			await using var ___ = await _lock.ScopeAsyncLock();
			if (_messageId == null)
				return;

			var client = await _kaliska.GetClient();
			var channel = await client.GetChannelAsync(_channelId);
			var message = await channel.GetMessageAsync(_messageId.Value);
			var builder = new UniversalMessageBuilder(message);
			sendJob = SendMessage(builder.NewWithDisabledComponents());
		}
		catch (NotFoundException)
		{
			return;
		}

		await sendJob;
	}

	public async Task<EventBus<TEvent>> GetSessionRelatedEvents<TEvent>() where TEvent : class
	{
		await using var ___ = await _lock.ScopeAsyncLock();

#pragma warning disable CS8603 // Possible null reference return.
		if (typeof(TEvent).IsEquivalentTo(typeof(MessageCreateEventArgs)))
		{
			var router = await _kaliska.GetEventRouter<MessageCreateEventArgs>();

			return (await router.PlaceRequest(x => {
				if (x.Message?.ChannelId == _channelId)
					return true;
				return false;
			})) as EventBus<TEvent>;
		}

		if (typeof(TEvent).IsEquivalentTo(typeof(MessageReactionAddEventArgs)))
		{
			var router = await _kaliska.GetEventRouter<MessageReactionAddEventArgs>();

			return (await router.PlaceRequest(x => {
				if (_messageId != null && x.Message?.Id == _messageId)
					return true;
				return false;
			})) as EventBus<TEvent>;
		}
#pragma warning restore CS8603 // Possible null reference return.
		throw new NotSupportedException();
	}

	public async Task RemoveMessage()
	{
		await using var ___ = await _lock.ScopeAsyncLock();
		if (_messageId == null)
			return;

		var client = await _kaliska.GetClient();
		try
		{
			var channel = await client.GetChannelAsync(_channelId);
			var message = await channel.GetMessageAsync(_messageId.Value);
			var reason = $"Deletion requested by {this.GetType().Name}";
			await channel.DeleteMessageAsync(message, reason);
		}
		catch (NotFoundException)
		{
			return;
		}
	}

	public async Task SendMessage(UniversalMessageBuilder content)
	{
		await using var ___ = await _lock.ScopeAsyncLock();
		if (!IsUsable)
			throw new Exception();

		var client = await _kaliska.GetClient();
		try
		{
			var channel = await client.GetChannelAsync(_channelId);
			if (_messageId == null)
			{
				var message = await channel.SendMessageAsync(content);
				_messageId = message.Id;
			}
			else
			{
				var message = await channel.GetMessageAsync(_messageId.Value);
				await message.ModifyAsync(x => {
					x.Clear();
					content.CopyTo(x);
				});
			}
		}
		catch (NotFoundException)
		{
			IsUsable = false;
			throw;
		}
	}

	public bool TryTransformAs<TNewSessionChannel>(out TNewSessionChannel channel) where TNewSessionChannel : IServiceProvider => throw new NotImplementedException();

	public bool CanTransformAs<TNewSessionChannel>() where TNewSessionChannel : IServiceProvider => throw new NotImplementedException();

	public TNewSessionChannel TransformAs<TNewSessionChannel>() where TNewSessionChannel : IServiceProvider => throw new NotImplementedException();
}
