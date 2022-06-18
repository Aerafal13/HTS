using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace HTS.Events;

public sealed record GuildMemberAddedNotification(DiscordClient Client, GuildMemberAddEventArgs Args) : INotification;

public sealed class GuildMemberAddedHandler : INotificationHandler<GuildMemberAddedNotification>
{
	private readonly IConfiguration _configuration;

	public GuildMemberAddedHandler(IConfiguration configuration) =>
		_configuration = configuration;

	public async Task Handle(GuildMemberAddedNotification notification, CancellationToken cancellationToken)
	{
		var welcomeChannel = notification.Args.Guild.GetChannel(
			_configuration.GetRequiredSection("channels")
			.GetValue<ulong>("welcome"));

		var presentationChannelId = _configuration.GetRequiredSection("channels")
			.GetValue<ulong>("presentation");

		var embed = new DiscordEmbedBuilder()
			.WithAuthor(notification.Args.Member.Username, iconUrl: notification.Args.Member.GetAvatarUrl(ImageFormat.Png))
			.WithDescription($@"Bonjour {notification.Args.Member.Mention} :wave: et bienvenue sur {notification.Args.Guild.Name}.
			N'hésite pas à te présenter dans le salon <#{presentationChannelId}>!")
			.WithColor(DiscordColor.Orange)
			.Build();

		await welcomeChannel.SendMessageAsync(embed);
	}
}
