using System.Threading.Channels;
using HTS.Core.Scheduling.Jobs;

namespace HTS.Core.Scheduling;

public sealed class Scheduler : IScheduler
{
	private readonly Channel<Job> _channel;
	private readonly PeriodicTimer _timer;
	private readonly CancellationTokenSource _cts;

	public Scheduler()
	{
		_channel = Channel.CreateUnbounded<Job>();
		_timer = new(TimeSpan.FromSeconds(1));
		_cts = new();
	}

	public async Task StartAsync()
	{
		while (await _timer.WaitForNextTickAsync(_cts.Token))
			await ExecuteAsync();
	}

	private async Task ExecuteAsync()
	{
		if (!_cts.IsCancellationRequested)
		{
			var reader = _channel.Reader ?? throw new ObjectDisposedException(ToString());
			var writer = _channel.Writer ?? throw new ObjectDisposedException(ToString());

			if (reader.Count > 0)
				await foreach (var job in reader.ReadAllAsync(_cts.Token))
				{
					if (job.IsAvailable())
						await job.ExecuteAsync(_cts.Token);

					if (!job.IsDelayed)
						await writer.WriteAsync(job, _cts.Token);
				}
		}
	}

	public async Task EnqueueAsync(Job job)
	{
		var writer = _channel.Writer ?? throw new ObjectDisposedException(ToString());

		await writer.WriteAsync(job, _cts.Token);
	}

	public void Dispose()
	{
		_channel.Writer.Complete();
		_cts.Cancel();
		_cts.Dispose();
		GC.SuppressFinalize(this);
	}
}
