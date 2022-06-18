using HTS.Models.Youtube;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace HTS.Services.Youtube;

public sealed class YoutubeVideoService : IYoutubeVideoService
{
	private readonly IMongoCollection<YoutubeVideo> _videosBook;

	public YoutubeVideoService(MongoClient client, IConfiguration configuration) =>
		_videosBook = client.GetDatabase(configuration["mongo_db:database"])
			.GetCollection<YoutubeVideo>(configuration["mongo_db:books:videos"]);

	public async Task<bool> IsLastVideoAsync(string videoId, CancellationToken cancellationToken = default)
	{
		var video = await _videosBook.FindAsync(x => x.VideoId == videoId, cancellationToken: cancellationToken);

		return video is not null;
	}

	public async Task<YoutubeVideo?> GetLastVideoAsync(CancellationToken cancellationToken = default)
	{
		var videos = await _videosBook.Find(_ => true).ToListAsync(cancellationToken);

		return videos.FirstOrDefault();
	}

	public Task InsertVideoAsync(YoutubeVideo video, CancellationToken cancellationToken = default) =>
		_videosBook.InsertOneAsync(video, cancellationToken: cancellationToken);

	public Task ReplaceLastVideoAsync(string lastVideoId, YoutubeVideo newVideo, CancellationToken cancellationToken = default) =>
		_videosBook.ReplaceOneAsync(x => x.VideoId == lastVideoId, newVideo, cancellationToken: cancellationToken);
}
