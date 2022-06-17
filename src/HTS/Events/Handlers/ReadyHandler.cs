using DSharpPlus.Entities;
using HTS.Events.Notifications;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HTS.Events.Handlers;

public sealed class ReadyHandler : MediatR.INotificationHandler<ReadyNotification>
{
	private readonly IConfiguration _configuration;

	public ReadyHandler(IConfiguration configuration) =>
		_configuration = configuration;

	public async Task Handle(ReadyNotification notification, CancellationToken cancellationToken)
	{
		var guild = await notification.Client.GetGuildAsync(_configuration.GetValue<ulong>("guild_id"));
		var members = await guild.GetAllMembersAsync();

		notification.Client.Logger.LogInformation("HTS Bot is ready in guild {GuildId} and counts {MembersCount} members.", guild.Id, members.Count);

		await notification.Client.UpdateStatusAsync(new($"{members.Count} membres", ActivityType.Watching), UserStatus.Online);
	}
}
