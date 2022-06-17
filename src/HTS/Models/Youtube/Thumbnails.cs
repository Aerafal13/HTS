using System.Text.Json.Serialization;

namespace HTS.Models.Youtube;

public sealed record Thumbnails(
	[property: JsonPropertyName("default")] Default Default,
	[property: JsonPropertyName("medium")] Medium Medium,
	[property: JsonPropertyName("high")] High High,
	[property: JsonPropertyName("standard")] Standard Standard,
	[property: JsonPropertyName("maxres")] Maxres MaxRes);