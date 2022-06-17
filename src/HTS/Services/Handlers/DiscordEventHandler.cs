using DSharpPlus;
using HTS.Core.DependencyInjection;
using HTS.Events.Notifications;
using MediatR;

namespace HTS.Services.Handlers;

/// <summary>
/// Represents a middleware between discord events and MediatR.
/// </summary>
public sealed class DiscordEventHandler : IHostedService
{
	private readonly DiscordClient _client;
	private readonly IMediator _mediator;

	public DiscordEventHandler(DiscordClient client, IMediator mediator) =>
		(_client, _mediator) = (client, mediator);

	public void Initialize()
	{
		_client.Ready += (sender, e) => _mediator.Publish(new ReadyNotification(sender, e));
		_client.GuildMemberAdded += (sender, e) => _mediator.Publish(new GuildMemberAddedNotification(sender, e));
	}
}
