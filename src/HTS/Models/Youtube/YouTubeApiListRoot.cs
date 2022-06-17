using System.Text.Json.Serialization;

namespace HTS.Models.Youtube;

public sealed record YouTubeApiListRoot(
	[property: JsonPropertyName("kind")] string Kind,
	[property: JsonPropertyName("etag")] string Etag,
	[property: JsonPropertyName("nextPageToken")] string NextPageToken,
	[property: JsonPropertyName("regionCode")] string RegionCode,
	[property: JsonPropertyName("items")] IEnumerable<YouTubeApiListItem> Items);