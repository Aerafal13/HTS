using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using HTS.Core.Extensions;
using HTS.Services.Users;
using HTS.Services.Youtube;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Sinks.SystemConsole.Themes;

var configuration = new ConfigurationBuilder()
	.SetBasePath(AppContext.BaseDirectory)
	.AddJsonFile("appsettings.json", false, true)
	.Build();

Log.Logger = new LoggerConfiguration()
	.Enrich.FromLogContext()
	.WriteTo.Async(x => x.Console(
		theme: AnsiConsoleTheme.Literate,
		outputTemplate: "({Timestamp:HH:mm:ss}) [{Level}] {SourceContext}: {Message:lj}{NewLine}{Exception}"))
	.CreateLogger();

var discordClient = new DiscordClient(new DiscordConfiguration
{
	Token = configuration["discord:token"],
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
	.AddSingleton<IYoutubeSearchService, YoutubeSearchService>()
	.AddTransient<IYoutubeVideoService, YoutubeVideoService>()
	.AddSingleton<IUserService, UserService>()
	.AddMediatR(x => x.AsSingleton(), assembly)
	.AddSingleton(new MongoClient(configuration["mongo_db:token"]))
	.AddScheduler()
	.AddHandlers()
	.BuildServiceProvider()
	.UseHandlers();

await discordClient.ConnectAsync();
await services.UseSchedulerAsync();