using DSharpPlus;
using DSharpPlus.EventArgs;
using MediatR;

namespace HTS.Events.Notifications;

public sealed record GuildMemberAddedNotification(DiscordClient Client, GuildMemberAddEventArgs Args) : INotification;
