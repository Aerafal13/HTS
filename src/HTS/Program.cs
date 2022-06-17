using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using HTS.Core.Extensions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Sinks.SystemConsole.Themes;

var configuration = new ConfigurationBuilder()
	.SetBasePath(AppContext.BaseDirectory)
	.AddJsonFile("appsettings.json", false, true)
	.Build();

Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Override("System", LogEventLevel.Warning)
	.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
	.MinimumLevel.Override("DSharpPlus", LogEventLevel.Information)
	.Enrich.FromLogContext()
	.WriteTo.Async(x => x.Console(
		theme: AnsiConsoleTheme.Literate,
		outputTemplate: "({Timestamp:HH:mm:ss}) [{Level}] {SourceContext}: {Message:lj}{NewLine}{Exception}"))
	.CreateLogger();

var discordClient = new DiscordClient(new DiscordConfiguration
{
	Token = configuration["token"],
	TokenType = TokenType.Bot,
	Intents = DiscordIntents.All,
	AlwaysCacheMembers = true,
	AutoReconnect = true,
	LoggerFactory = new SerilogLoggerFactory()
});

var assembly = typeof(Program).Assembly;

var services = new ServiceCollection()
	.AddSingleton<IConfiguration>(configuration)
	.AddSingleton(discordClient)
	.AddSingleton(discordClient.UseInteractivity(new InteractivityConfiguration { Timeout = TimeSpan.FromMinutes(2) }))
	.AddSingleton(x => discordClient.UseSlashCommands(new SlashCommandsConfiguration { Services = x }))
	.AddMediatR(x => x.AsSingleton(), assembly)
	.AddHandlers()
	.BuildServiceProvider();

services.GetRequiredService<SlashCommandsExtension>()
	.RegisterCommands(assembly, configuration.GetValue<ulong>("guild_id"));

services.UseHandlers();

await services.UseSchedulerAsync();

await discordClient.ConnectAsync();

await Task.Delay(Timeout.Infinite);