namespace HTS.Core.DependencyInjection;

/// <summary>
/// Defines methods for objects that are managed by the <see cref="IServiceProvider"/>.
/// </summary>
public interface IHostedService
{
	/// <summary>
	/// Triggered when the application is ready to start the service.
	/// </summary>
	void Initialize();
}
