using System.Text.Json.Serialization;

namespace HTS.Models.Youtube;

public sealed record YouTubeApiDetailsItem(
		[property: JsonPropertyName("kind")] string Kind,
		[property: JsonPropertyName("etag")] string Etag,
		[property: JsonPropertyName("id")] string Id,
		[property: JsonPropertyName("snippet")] Snippet Snippet,
		[property: JsonPropertyName("statistics")] Statistics Statistics);