using System.Text.Json.Serialization;

namespace HTS.Models.Youtube;

public sealed record YouTubeApiListId(
	[property: JsonPropertyName("kind")] string Kind,
	[property: JsonPropertyName("videoId")] string VideoId);