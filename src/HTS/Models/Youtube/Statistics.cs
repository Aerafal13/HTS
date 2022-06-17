using System.Text.Json.Serialization;

namespace HTS.Models.Youtube;

public sealed record Statistics(
	[property: JsonPropertyName("viewCount")] int ViewCount,
	[property: JsonPropertyName("likeCount")] int LikeCount,
	[property: JsonPropertyName("dislikeCount")] int DislikeCount,
	[property: JsonPropertyName("favoriteCount")] int FavoriteCount,
	[property: JsonPropertyName("commentCount")] int CommentCount);