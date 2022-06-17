using DSharpPlus;
using DSharpPlus.EventArgs;
using MediatR;

namespace HTS.Events.Notifications;

public sealed record ReadyNotification(DiscordClient Client, ReadyEventArgs Args) : INotification;
