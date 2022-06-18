using HTS.Models.Youtube;

namespace HTS.Services.Youtube;

/// <summary>
/// Represents a youtube video repository.
/// </summary>
public interface IYoutubeVideoService
{
	/// <summary>
	/// Checks whether the <paramref name="videoId"/> correspond to the last youtube video id.
	/// </summary>
	/// <param name="videoId">The video id.</param>
	/// <param name="cancellationToken">Triggered when the application is closed.</param>
	/// <returns>Whether the <paramref name="videoId"/> correspond to the last youtube video id.</returns>
	Task<bool> IsLastVideoAsync(string videoId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets the last youtube video.
	/// </summary>
	/// <param name="cancellationToken">Triggered when the application is closed.</param>
	/// <returns>The last youtube video.</returns>
	Task<YoutubeVideo?> GetLastVideoAsync(CancellationToken cancellationToken = default);

	Task InsertVideoAsync(YoutubeVideo video, CancellationToken cancellationToken = default);

	/// <summary>
	/// Replaces the last youtube video per the <paramref name="newVideo"/>.
	/// </summary>
	/// <param name="lastVideoId">The last video id.</param>
	/// <param name="newVideo">The new video to insert.</param>
	/// <param name="cancellationToken">Triggered when the application is closed.</param>
	Task ReplaceLastVideoAsync(string lastVideoId, YoutubeVideo newVideo, CancellationToken cancellationToken = default);
}
