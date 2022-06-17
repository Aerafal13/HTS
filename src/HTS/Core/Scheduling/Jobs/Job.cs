namespace HTS.Core.Scheduling.Jobs;

/// <summary>
/// Represents an asynchronous execution with conditions.
/// </summary>
public sealed class Job
{
	private readonly Func<CancellationToken, Task> _asyncCallback;
	private readonly TimeSpan _delay;
	private readonly Func<bool>? _predicate;

	private DateTime _time;

	/// <summary>
	/// Gets whether the job is delayed.
	/// </summary>
	public bool IsDelayed { get; }

	public Job(Func<CancellationToken, Task> asyncCallback, TimeSpan delay, bool isDelayed, Func<bool>? predicate)
	{
		_asyncCallback = asyncCallback;
		_delay = delay;
		_predicate = predicate;
		_time = DateTime.Now;
		IsDelayed = isDelayed;
	}

	/// <returns>Whether the job is ready to execute.</returns>
	public bool IsAvailable()
	{
		if (_predicate is not null)
			if (!_predicate())
				return false;

		return _time <= DateTime.Now;
	}

	/// <summary>
	/// Executes the job in asynchronously.
	/// </summary>
	/// <param name="cancellationToken">Triggered when the application stopping.</param>
	public Task ExecuteAsync(CancellationToken cancellationToken)
	{
		_time = DateTime.Now + _delay;

		return _asyncCallback(cancellationToken);
	}
}