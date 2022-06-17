using System.Text.Json.Serialization;

namespace HTS.Models.Youtube;
public sealed record Localized(
	[property: JsonPropertyName("title")] string Title,
	[property: JsonPropertyName("description")] string Description);