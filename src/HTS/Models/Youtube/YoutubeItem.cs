namespace HTS.Models.Youtube;

public sealed record YoutubeItem(
	string VideoId,
	string Title,
	string Description,
	string ChannelTitle,
	string DefaultThumbnailUrl,
	string MediumThumbnailUrl,
	string HighThumbnailUrl,
	string StandardThumbnailUrl,
	string MaxResThumbnailUrl,
	DateTime PublishedAt,
	int ViewCount,
	int LikeCount,
	int DislikeCount,
	int FavoriteCount,
	int CommentCount,
	IEnumerable<string> Tags)
{
	public string ShortDescription =>
	   Description.Length > 150
		   ? string.Concat(Description.AsSpan(0, 150), "...")
		   : Description;
}