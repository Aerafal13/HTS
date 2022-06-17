﻿using System.Text.Json.Serialization;

namespace HTS.Models.Youtube;

public sealed record Default(
	[property: JsonPropertyName("url")] string Url,
	[property: JsonPropertyName("width")] int Width,
	[property: JsonPropertyName("height")] int Height);