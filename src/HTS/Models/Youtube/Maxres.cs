using System.Text.Json.Serialization;

namespace HTS.Models.Youtube;

public sealed record Maxres(
	[property: JsonPropertyName("url")] string Url,
	[property: JsonPropertyName("width")] int Width,
	[property: JsonPropertyName("height")] int Height);