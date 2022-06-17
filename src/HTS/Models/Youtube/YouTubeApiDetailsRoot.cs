using System.Text.Json.Serialization;

namespace HTS.Models.Youtube;

public sealed record YouTubeApiDetailsRoot(
	[property: JsonPropertyName("kind")] string Kind,
	[property: JsonPropertyName("etag")] string Etag,
	[property: JsonPropertyName("items")] IEnumerable<YouTubeApiDetailsItem> Items);