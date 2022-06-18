using System.Net.Http.Json;
using HTS.Models.Youtube;
using Microsoft.Extensions.Configuration;

namespace HTS.Services;

public sealed class YoutubeService : IDisposable
{
	private readonly HttpClient _client;
	private readonly IConfiguration _configuration;

	private const string ChannelId = "UCBu6vkOw_tMx_mWNKTpnv-w";
	private const string ChannelUrl = "https://www.googleapis.com/youtube/v3/search?part=id&maxResults=1&order=date&channelId={0}&key={1}";
	private const string DetailsUrl = "https://www.googleapis.com/youtube/v3/videos?part=snippet,statistics&key={0}&id={1}";

	public YoutubeService(IConfiguration configuration) =>
		(_client, _configuration) = (new(), configuration);

	public async Task<YoutubeItem> GetLastVideoAsync()
	{
		var key = _configuration.GetConnectionString("youtube_token");
		var rootUrl = string.Format(ChannelUrl, ChannelId, key);

		var response = await _client.GetAsync(rootUrl);
		response.EnsureSuccessStatusCode();

		var root = await response.Content.ReadFromJsonAsync<YouTubeApiListRoot>();
		var detailsRootUrl = string.Format(DetailsUrl, key, string.Join(',', root!.Items.Select(x => x.Id.VideoId)));

		var detailsResponse = await _client.GetAsync(detailsRootUrl);
		detailsResponse.EnsureSuccessStatusCode();

		var rootDetails = await detailsResponse.Content.ReadFromJsonAsync<YouTubeApiDetailsRoot>();

		var item = rootDetails!.Items.First();

		return new YoutubeItem(
					item.Id,
					item.Snippet.Title,
					item.Snippet.Description,
					item.Snippet.ChannelTitle,
					item.Snippet.Thumbnails.Default.Url,
					item.Snippet.Thumbnails.Medium.Url,
					item.Snippet.Thumbnails.High.Url,
					item.Snippet.Thumbnails.Standard.Url,
					item.Snippet.Thumbnails.MaxRes.Url,
					item.Snippet.PublishedAt,
					item.Statistics.ViewCount,
					item.Statistics.LikeCount,
					item.Statistics.DislikeCount,
					item.Statistics.FavoriteCount,
					item.Statistics.CommentCount,
					item.Snippet.Tags);
	}

	public void Dispose()
	{
		_client.Dispose();
		GC.SuppressFinalize(this);
	}
}
