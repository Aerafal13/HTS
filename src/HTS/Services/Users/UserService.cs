using DSharpPlus.Entities;
using HTS.Models.Users;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace HTS.Services.Users;

public sealed class UserService : IUserService
{
	private readonly IMongoCollection<User> _usersBook;
	private readonly uint _experienceRate;

	public UserService(MongoClient client, IConfiguration configuration)
	{
		_usersBook = client.GetDatabase(configuration["mongo_db:database"]).GetCollection<User>(configuration["mongo_db:books:users"]);
		_experienceRate = configuration.GetRequiredSection("discord").GetValue<uint>("experience_rate");
	}

	public Task CreateUserAsync(User user, CancellationToken cancellationToken = default) =>
		_usersBook.InsertOneAsync(user, cancellationToken: cancellationToken);

	public async Task<bool> UserExistAsync(ulong discordId, CancellationToken cancellationToken = default)
	{
		var user = await _usersBook.Find(x => x.DiscordId == discordId).FirstOrDefaultAsync(cancellationToken: cancellationToken);

		return user is not null;
	}

	public Task<User> GetUserAsync(ulong discordId, CancellationToken cancellationToken = default) =>
		 _usersBook.Find(x => x.DiscordId == discordId).FirstOrDefaultAsync(cancellationToken: cancellationToken);

	public async Task UpdateUserExperienceAsync(User user, DiscordChannel channel, DiscordUser author, ulong experience, CancellationToken cancellationToken = default)
	{
		user.Experience += experience * _experienceRate;

		if (user.Experience >= user.ExperienceToNextLevel)
		{
			user.Experience -= user.ExperienceToNextLevel;
			user.Level++;

			var embed = new DiscordEmbedBuilder()
					.WithColor(DiscordColor.Orange)
					.WithDescription($"Bien joué {author.Mention}, tu es maintenant niveau {user.Level}!")
					.Build();

			await channel.SendMessageAsync(embed);
		}

		await _usersBook.ReplaceOneAsync(x => x.DiscordId == user.DiscordId, user, cancellationToken: cancellationToken);
	}
}
