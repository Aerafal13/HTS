using HTS.Core.Scheduling.Jobs;

namespace HTS.Core.Scheduling;

/// <summary>
/// Represents a set of instructions to be dispatched asynchronously.
/// </summary>
public interface IScheduler : IDisposable
{
	/// <summary>
	/// Triggered when the application host is ready to start the service.
	/// </summary>
	Task StartAsync();

	/// <summary>
	/// Allows you to add a <see cref="Job"/>.
	/// </summary>
	/// <param name="job">The execution.</param>
	Task EnqueueAsync(Job job);
}