namespace HTS.Core.Scheduling.Jobs;

/// <summary>
/// Constructs an empty <see cref="JobBuilder"/>.
/// </summary>
public sealed class JobBuilder
{
	private readonly Func<CancellationToken, Task> _asyncCallback;
	private TimeSpan _delay;
	private bool _isDelayed;

	private Func<bool>? _predicate;

	private JobBuilder(Func<CancellationToken, Task> asyncCallback) =>
		_asyncCallback = asyncCallback;

	/// <summary>
	/// Creates a new <see cref="JobBuilder"/>.
	/// </summary>
	/// <param name="asyncCallback">The callback to execute.</param>
	/// <returns>The current instance of <see cref="JobBuilder"/>.</returns>
	public static JobBuilder CreateJob(Func<CancellationToken, Task> asyncCallback) =>
		new(asyncCallback);

	/// <summary>
	/// Sets the filter property.
	/// </summary>
	/// <param name="predicate">The predicate to execute before the callback.</param>
	/// <returns>The current instance of <see cref="JobBuilder"/>.</returns>
	public JobBuilder Filter(Func<bool> predicate)
	{
		_predicate = predicate;

		return this;
	}

	/// <summary>
	/// Sets if the job is running delayed.
	/// </summary>
	/// <param name="delay">The delay to wait.</param>
	/// <returns>The current instance of <see cref="JobBuilder"/>.</returns>
	public JobBuilder RunAsDelayed(TimeSpan delay)
	{
		_delay = delay;
		_isDelayed = true;

		return this;
	}

	/// <summary>
	/// Sets if the job is running periodically.
	/// </summary>
	/// <param name="delay">The delay to wait.</param>
	/// <returns>The current instance of <see cref="JobBuilder"/>.</returns>
	public JobBuilder RunAsPeriodically(TimeSpan delay)
	{
		_delay = delay;

		return this;
	}

	/// <summary>
	/// Creates a new instance of <see cref="Job"/>.
	/// </summary>
	/// <returns>A new instance of <see cref="Job"/>.</returns>
	/// <exception cref="Exception">Whether the run mode is not called.</exception>
	public Job Build()
	{
		if (_delay == TimeSpan.Zero)
			throw new Exception($"The delay cannot be null, call {nameof(RunAsDelayed)} or {nameof(RunAsPeriodically)} before calling {nameof(Build)}.");

		return new(_asyncCallback, _delay, _isDelayed, _predicate);
	}
}
