using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using HTS.Core.Extensions;
using HTS.Services;

namespace HTS.Commands;

[SlashCommandGroup("Utility", "Provides utility commands.")]
public sealed class UtilityCommands : ApplicationCommandModule
{
	private readonly YoutubeService _youtube;

	public UtilityCommands(YoutubeService youtube) =>
		_youtube = youtube;

	[SlashCommand("Generate", "Generate all video embeds.")]
	public async Task GenerateVideosEmbedAsync(InteractionContext ctx)
	{
		var youtubeVideo = await _youtube.GetLastVideoAsync();

		var embed = new DiscordEmbedBuilder()
				.WithAuthor($"Nouvelle vidéo de {youtubeVideo.ChannelTitle} !")
				.WithTitle(string.Concat("**", youtubeVideo.Title, "**"))
				.WithDescription(youtubeVideo.Description)
				.WithUrl(youtubeVideo.VideoUrl)
				.WithImageUrl(youtubeVideo.MaxResThumbnailUrl)
				.WithColor(new DiscordColor(54, 57, 63))
				.WithFooter(string.Concat("Publiée le ", youtubeVideo.PublishedAt.ToHumanDate()), ctx.Client.CurrentUser.AvatarUrl);

		await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
				.AddEmbed(embed));
	}
}
