using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;
using HTS.Core.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HTS.Services.Handlers;

/// <summary>
/// Covers the handling of discord slash commands and treats them accordingly.
/// </summary>
public sealed class DiscordCommandHandler : IHostedService
{
	private readonly SlashCommandsExtension _slashCommands;

	public DiscordCommandHandler(SlashCommandsExtension slashCommands) =>
		_slashCommands = slashCommands;

	public void Initialize()
	{
		_slashCommands.SlashCommandErrored += OnSlashCommandErroredAsync;
		_slashCommands.SlashCommandExecuted += OnSlashCommandExecutedAsync;

		_slashCommands.ContextMenuErrored += OnContextMenuErroredAsync;
		_slashCommands.ContextMenuExecuted += OnContextMenuExecutedAsync;
	}

	private Task OnContextMenuExecutedAsync(SlashCommandsExtension sender, ContextMenuExecutedEventArgs e)
	{
		sender.Client.Logger.LogInformation("ContextMenu: {UserName} used command '{CommandName}'.", e.Context.User, e.Context.CommandName);

		return Task.CompletedTask;
	}

	private Task OnContextMenuErroredAsync(SlashCommandsExtension sender, ContextMenuErrorEventArgs e)
	{
		sender.Client.Logger.LogInformation("ContextMenu: {UserName} failed to use command '{CommandName}' with error: {Error}.",
			e.Context.User, e.Context.CommandName, e.Exception.Message);

		return Task.CompletedTask;
	}

	private Task OnSlashCommandExecutedAsync(SlashCommandsExtension sender, SlashCommandExecutedEventArgs e)
	{
		sender.Client.Logger.LogInformation("SlashCommand: {UserName} used command '{CommandName}'.", e.Context.User, e.Context.CommandName);

		return Task.CompletedTask;
	}

	private Task OnSlashCommandErroredAsync(SlashCommandsExtension sender, SlashCommandErrorEventArgs e)
	{
		sender.Client.Logger.LogInformation("SlashCommand: {UserName} failed to use command '{CommandName}' with error: {Error}.",
				e.Context.User, e.Context.CommandName, e.Exception.Message);

		return Task.CompletedTask;
	}
}
