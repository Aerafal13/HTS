using System.Text.Json.Serialization;

namespace HTS.Models.Youtube;

public sealed record Snippet(
	[property: JsonPropertyName("publishedAt")] DateTime PublishedAt,
	[property: JsonPropertyName("channelId")] string ChannelId,
	[property: JsonPropertyName("title")] string Title,
	[property: JsonPropertyName("description")] string Description,
	[property: JsonPropertyName("thumbnails")] Thumbnails Thumbnails,
	[property: JsonPropertyName("channelTitle")] string ChannelTitle,
	[property: JsonPropertyName("tags")] IEnumerable<string> Tags,
	[property: JsonPropertyName("categoryId")] string CategoryId,
	[property: JsonPropertyName("liveBroadcastContent")] string LiveBroadcastContent,
	[property: JsonPropertyName("defaultLanguage")] string DefaultLanguage,
	[property: JsonPropertyName("localized")] Localized Localized,
	[property: JsonPropertyName("defaultAudioLanguage")] string DefaultAudioLanguage);