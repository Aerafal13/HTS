using MongoDB.Bson.Serialization.Attributes;

namespace HTS.Models.Youtube;

public sealed record YoutubeVideo(
	[property: BsonId] string VideoId,
	string Title,
	string Description,
	string ChannelTitle,
	string MaxResThumbnailUrl,
	DateTime PublishedAt)
{
	[BsonIgnore]
	public string VideoUrl =>
		string.Concat("https://www.youtube.com/watch?v=", VideoId);

	[BsonIgnore]
	public string ChannelUrl =>
		string.Concat("https://www.youtube.com/c/", ChannelTitle);

	[BsonIgnore]
	public string ShortDescription =>
	   Description.Length > 150
		   ? string.Concat(Description.AsSpan(0, 150), "...")
		   : Description;
}