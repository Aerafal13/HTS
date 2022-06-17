using HTS.Core.DependencyInjection;
using HTS.Services.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace HTS.Core.Extensions;

/// <summary>
/// Extensions methods for manage the <see cref="IServiceCollection"/> and <seealso cref="IServiceProvider"/>.
/// </summary>
public static class DependencyInjectionExtensions
{
	/// <summary>
	/// Registers handler types into the <paramref name="services"/>.
	/// </summary>
	/// <param name="services">The service collection.</param>
	/// <returns>The service collection.</returns>
	public static IServiceCollection AddHandlers(this IServiceCollection services) =>
		services
			.AddSingleton<IHostedService, DiscordEventHandler>()
			.AddSingleton<IHostedService, DiscordCommandHandler>();

	/// <summary>
	/// Finds all handler types into the <paramref name="provider"/> and call the <see cref="IHostedService.Initialize"/> method.
	/// </summary>
	/// <param name="provider">The service provider.</param>
	public static void UseHandlers(this IServiceProvider provider)
	{
		foreach (var service in provider.GetServices<IHostedService>())
			service.Initialize();
	}
}
