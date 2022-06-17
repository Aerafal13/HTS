using DSharpPlus.Entities;
using HTS.Events.Notifications;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace HTS.Events.Handlers;

public sealed class ReadyHandler : INotificationHandler<ReadyNotification>
{
	private readonly IConfiguration _configuration;

	public ReadyHandler(IConfiguration configuration) =>
		_configuration = configuration;

	public async Task Handle(ReadyNotification notification, CancellationToken cancellationToken)
	{
		var guild = await notification.Client.GetGuildAsync(_configuration.GetValue<ulong>("guild_id"), true);

		await notification.Client.UpdateStatusAsync(new($"{guild.MemberCount} membres", ActivityType.Watching), UserStatus.Online);
	}
}
