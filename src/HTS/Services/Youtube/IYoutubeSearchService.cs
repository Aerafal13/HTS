using DSharpPlus;
using HTS.Models.Youtube;

namespace HTS.Services.Youtube;

/// <summary>
/// Represents a youtube video finder.
/// </summary>
public interface IYoutubeSearchService
{
	/// <summary>
	/// Finds the last youtube video from a channel id.
	/// </summary>
	/// <param name="cancellationToken">Triggered when the application is closed.</param>
	/// <returns>A youtube video results.</returns>
	Task<YoutubeVideo> GetLastVideoAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Initializes a periodically job, finds the last video and compare them.
	/// </summary>
	/// <param name="client">The discord client.</param>
	Task InitializeVideoJobAsync(DiscordClient client);
}
