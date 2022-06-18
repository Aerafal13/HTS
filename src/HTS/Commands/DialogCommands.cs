using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace HTS.Commands;

[SlashCommandGroup("Dialog", "Provides methods to create a dialog into a channel.")]
public sealed class DialogCommands : ApplicationCommandModule
{
	[SlashCommand("Voice", "Generate a dialog for manage voice channels.")]
	[SlashCommandPermissions(Permissions.Administrator)]
	public async Task CreateVoiceDialogAsync(InteractionContext ctx,
		[Option("channel_name", "The name of the channel to write the dialog")] DiscordChannel channel)
	{
		var voiceEmbed = new DiscordEmbedBuilder()
			.WithColor(new DiscordColor(54, 57, 63))
			.WithDescription("Tu peux créer un salon vocal, pour cela rien de plus simple :\nAppuie sur le boutton ci-dessous.")
			.Build();

		var button = new DiscordButtonComponent(ButtonStyle.Primary, "voice_button", "Créer un salon vocal", false, new(DiscordEmoji.FromName(ctx.Client, ":sound:")));

		await channel.SendMessageAsync(new DiscordMessageBuilder()
			.AddEmbed(voiceEmbed)
			.AddComponents(button));

		var responseEmbed = new DiscordEmbedBuilder()
			.WithColor(new DiscordColor(54, 57, 63))
			.WithDescription("Le dialog de création de salon vocal à bien été créer. :white_check_mark:")
			.Build();

		await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
			.AddEmbed(responseEmbed));
	}

	[SlashCommand("Notification", "Generate a dialog for manage notifications.")]
	[SlashCommandPermissions(Permissions.Administrator)]
	public async Task CreateNotificationDialogAsync(InteractionContext ctx,
		[Option("channel_name", "The name of the channel to write the dialog")] DiscordChannel channel)
	{
		var options = new DiscordSelectComponentOption[]
		{
			new("Annonces", "select_annonces", "0 membres ont activés cette notification.", false, new(DiscordEmoji.FromName(ctx.Client, ":loudspeaker:"))),
			new("Giveaway", "select_giveaway", "0 membres ont activés cette notification.", false, new(DiscordEmoji.FromName(ctx.Client, ":tada:"))),
			new("Suggestions", "select_suggestions", "0 membres ont activés cette notification.", false, new(DiscordEmoji.FromName(ctx.Client, ":bulb:"))),
			new("Évenements", "select_events", "0 membres ont activés cette notification.", false, new(DiscordEmoji.FromName(ctx.Client, ":zap:"))),
			new("Sondages", "select_sondages", "0 membres ont activés cette notification.", false, new(DiscordEmoji.FromName(ctx.Client, ":bar_chart:"))),
			new("Youtube", "select_youtube", "0 membres ont activés cette notification.", false, new(DiscordEmoji.FromName(ctx.Client, ":movie_camera:"))),
		};

		var component = new DiscordSelectComponent("notifications", "Selectionner une action", options);

		var notifEmbed = new DiscordEmbedBuilder()
			.WithColor(new DiscordColor(54, 57, 63))
			.WithTitle("Activer des notifications :")
			.WithDescription("Obtiens le rôle lié à un type de notification pour rester informé des actualités !")
			.Build();

		await channel.SendMessageAsync(new DiscordMessageBuilder()
			.AddEmbed(notifEmbed)
			.AddComponents(component));

		var responseEmbed = new DiscordEmbedBuilder()
			.WithColor(new DiscordColor(54, 57, 63))
			.WithDescription("Le dialog de management de notifications à bien été créer. :white_check_mark:")
			.Build();

		await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
			.AddEmbed(responseEmbed));


	}
}
