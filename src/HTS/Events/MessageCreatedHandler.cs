using DSharpPlus;
using DSharpPlus.EventArgs;
using HTS.Models.Users;
using HTS.Services.Users;
using MediatR;

namespace HTS.Events;

public sealed record MessageCreatedNotification(DiscordClient Client, MessageCreateEventArgs Args) : INotification;

public sealed class MessageCreatedHandler : INotificationHandler<MessageCreatedNotification>
{
	private readonly IUserService _users;

	public MessageCreatedHandler(IUserService users) =>
		_users = users;

	public async Task Handle(MessageCreatedNotification notification, CancellationToken cancellationToken)
	{
		if (!notification.Args.Author.IsBot)
		{
			User? user;

			if (!await _users.UserExistAsync(notification.Args.Author.Id, cancellationToken))
			{
				user = new() { DiscordId = notification.Args.Author.Id, Experience = 0, Level = 1 };

				await _users.CreateUserAsync(user, cancellationToken);
			}
			else
				user = await _users.GetUserAsync(notification.Args.Author.Id, cancellationToken);

			if (user is not null)
				await _users.UpdateUserExperienceAsync(user, notification.Args.Channel, notification.Args.Author, 7, cancellationToken);
		}
	}
}
