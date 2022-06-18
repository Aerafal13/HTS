using MongoDB.Bson.Serialization.Attributes;

namespace HTS.Models.Users;

public sealed record User
{
	[BsonId]
	public ulong DiscordId { get; set; }

	public ulong Experience { get; set; }

	public uint Level { get; set; }

	[BsonIgnore]
	public ulong ExperienceToNextLevel =>
		150 * (Level * 2);
}