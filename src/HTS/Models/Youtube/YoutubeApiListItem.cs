using System.Text.Json.Serialization;

namespace HTS.Models.Youtube;

public sealed record YouTubeApiListItem(
	[property: JsonPropertyName("kind")] string Kind,
	[property: JsonPropertyName("etag")] string Etag,
	[property: JsonPropertyName("id")] YouTubeApiListId Id);