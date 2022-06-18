using System.Net.Http.Json;
using DSharpPlus;
using DSharpPlus.Entities;
using HTS.Core.Extensions;
using HTS.Core.Scheduling;
using HTS.Core.Scheduling.Jobs;
using HTS.Models.Youtube;
using Microsoft.Extensions.Configuration;

namespace HTS.Services.Youtube;

public sealed class YoutubeSearchService : IYoutubeSearchService
{
	private readonly IConfiguration _configuration;
	private readonly IScheduler _scheduler;
	private readonly IYoutubeVideoService _youtubeRepository;

	private const string ChannelUrl = "https://www.googleapis.com/youtube/v3/search?part=id&maxResults=1&order=date&channelId={0}&key={1}";
	private const string DetailsUrl = "https://www.googleapis.com/youtube/v3/videos?part=snippet,statistics&key={0}&id={1}";

	public YoutubeSearchService(IConfiguration configuration, IScheduler scheduler, IYoutubeVideoService youtubeRepository) =>
		(_configuration, _scheduler, _youtubeRepository) = (configuration, scheduler, youtubeRepository);

	public async Task<YoutubeVideo> GetLastVideoAsync(CancellationToken cancellationToken = default)
	{
		using var client = new HttpClient();

		var key = _configuration["youtube:token"];
		var rootUrl = string.Format(ChannelUrl, _configuration["youtube:channel_id"], key);

		var response = await client.GetAsync(rootUrl, cancellationToken);
		response.EnsureSuccessStatusCode();

		var root = await response.Content.ReadFromJsonAsync<YouTubeApiListRoot>(cancellationToken: cancellationToken);
		var detailsRootUrl = string.Format(DetailsUrl, key, string.Join(',', root!.Items.Select(x => x.Id.VideoId)));

		var detailsResponse = await client.GetAsync(detailsRootUrl, cancellationToken);
		detailsResponse.EnsureSuccessStatusCode();

		var rootDetails = await detailsResponse.Content.ReadFromJsonAsync<YouTubeApiDetailsRoot>(cancellationToken: cancellationToken);
		var item = rootDetails!.Items.First();

		return new(item.Id, item.Snippet.Title, item.Snippet.Description, item.Snippet.ChannelTitle, item.Snippet.Thumbnails.MaxRes.Url, item.Snippet.PublishedAt);
	}

	public async Task InitializeVideoJobAsync(DiscordClient client) =>
		await _scheduler.EnqueueAsync(JobBuilder.CreateJob(async cancellationToken =>
			{
				var searchedVideo = await GetLastVideoAsync(cancellationToken);
				var lastDataVideo = await _youtubeRepository.GetLastVideoAsync(cancellationToken);

				if (lastDataVideo is null)
				{
					await _youtubeRepository.InsertVideoAsync(searchedVideo, cancellationToken);
					await SendVideoEmbedAsync(searchedVideo, client);
				}
				else if (!await _youtubeRepository.IsLastVideoAsync(searchedVideo.VideoId, cancellationToken))
				{
					await _youtubeRepository.ReplaceLastVideoAsync(lastDataVideo.VideoId, searchedVideo, cancellationToken);
					await SendVideoEmbedAsync(searchedVideo, client);
				}
			})
			.Filter(() => DateTime.Now is { DayOfWeek: DayOfWeek.Wednesday, Hour: > 12, Minute: > 40 })
			.RunAsPeriodically(TimeSpan.FromMinutes(30))
			.Build());

	private async Task SendVideoEmbedAsync(YoutubeVideo video, DiscordClient client)
	{
		var channel = await client.GetChannelAsync(_configuration
							.GetRequiredSection("discord")
							.GetRequiredSection("channels")
							.GetValue<ulong>("video"));

		var embed = new DiscordEmbedBuilder()
			.WithAuthor($"Nouvelle vidéo de {video.ChannelTitle} !")
			.WithTitle(string.Concat("**", video.Title, "**"))
			.WithDescription(video.Description)
			.WithUrl(video.VideoUrl)
			.WithImageUrl(video.MaxResThumbnailUrl)
			.WithColor(new DiscordColor(54, 57, 63))
			.WithFooter(string.Concat("Publiée le ", video.PublishedAt.ToHumanDate()), client.CurrentUser.AvatarUrl);

		await channel.SendMessageAsync(embed);
	}
}
