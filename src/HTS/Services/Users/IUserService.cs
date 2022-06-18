using DSharpPlus.Entities;
using HTS.Models.Users;

namespace HTS.Services.Users;

/// <summary>
/// Represents a user repository.
/// </summary>
public interface IUserService
{
	/// <summary>
	/// Inserts a new user.
	/// </summary>
	/// <param name="user">The new user.</param>
	/// <param name="cancellationToken">Triggered when the application is closed.</param>
	Task CreateUserAsync(User user, CancellationToken cancellationToken = default);

	/// <summary>
	/// Checks whether the user already exists.
	/// </summary>
	/// <param name="discordId">The discord id.</param>
	/// <param name="cancellationToken">Triggered when the application is closed.</param>
	/// <returns>Whether the user already exists.</returns>
	Task<bool> UserExistAsync(ulong discordId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a user based on its discord id.
	/// </summary>
	/// <param name="discordId">The discord id.</param>
	/// <param name="cancellationToken">Triggered when the application is closed.</param>
	/// <returns>A user.</returns>
	Task<User> GetUserAsync(ulong discordId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates the user experience, add a new level if needable and send a discord embed.
	/// </summary>
	/// <param name="user">The user.</param>
	/// <param name="channel">The discord channel.</param>
	/// <param name="author">The author of the discord message.</param>
	/// <param name="experience">The added experience.</param>
	/// <param name="cancellationToken">Triggered when the application is closed.</param>
	Task UpdateUserExperienceAsync(User user, DiscordChannel channel, DiscordUser author, ulong experience, CancellationToken cancellationToken = default);
}
