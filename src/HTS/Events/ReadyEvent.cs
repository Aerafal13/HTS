using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using HTS.Services.Youtube;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HTS.Events;

public sealed record ReadyNotification(DiscordClient Client, ReadyEventArgs Args) : INotification;

public sealed class ReadyEvent : INotificationHandler<ReadyNotification>
{
	private readonly IConfiguration _configuration;
	private readonly IYoutubeSearchService _youtube;

	public ReadyEvent(IConfiguration configuration, IYoutubeSearchService youtube) =>
		(_configuration, _youtube) = (configuration, youtube);

	public async Task Handle(ReadyNotification notification, CancellationToken cancellationToken)
	{
		var guild = await notification.Client.GetGuildAsync(_configuration
			.GetRequiredSection("discord")
			.GetValue<ulong>("guild_id"));

		var members = await guild.GetAllMembersAsync();

		notification.Client.Logger.LogInformation("HTS Bot is ready in guild {GuildId} and counts {MembersCount} members.", guild.Id, members.Count);

		await notification.Client.UpdateStatusAsync(new($"{members.Count} membres", ActivityType.Watching), UserStatus.Online);

		await _youtube.InitializeVideoJobAsync(notification.Client);
	}
}
