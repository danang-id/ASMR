//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// TimedHostedService.cs
//

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// ReSharper disable once InvalidXmlDocComment
namespace ASMR.Web.Services.Generic;

/// <summary>
/// Based on Microsoft.Extensions.Hosting.BackgroundService  https://github.com/aspnet/Extensions/blob/master/src/Hosting/Abstractions/src/BackgroundService.cs
/// Additional info: - https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-2.2&tabs=visual-studio#timed-background-tasks
///                  - https://stackoverflow.com/questions/53844586/async-timer-in-scheduler-background-service
/// </summary>
public abstract class TimedHostedService : IHostedService, IDisposable
{
	private readonly CancellationTokenSource _cancellationTokenSource;
	private readonly TimeSpan _timeSpan;

	private Task _executingTask;
	private Timer _timer;

	protected readonly ILogger Logger;
	protected readonly string Name;
	protected readonly IServiceScopeFactory ServiceScopeFactory;

	protected TimedHostedService(
		string name,
		TimeSpan timeSpan,
		IServiceScopeFactory serviceScopeFactory,
		ILogger logger)
	{
		_cancellationTokenSource = new CancellationTokenSource();
		_timeSpan = timeSpan;

		Logger = logger;
		Name = name;
		ServiceScopeFactory = serviceScopeFactory;
	}

	public Task StartAsync(CancellationToken cancellationToken)
	{
		Logger.LogInformation("{Name} is starting", Name);

		_timer = new Timer(ExecuteTask, null, _timeSpan, TimeSpan.FromMilliseconds(1));

		return Task.CompletedTask;
	}

	private void ExecuteTask(object state)
	{
		_timer?.Change(Timeout.Infinite, 0);
		_executingTask = ExecuteTaskAsync(_cancellationTokenSource.Token);
	}

	private async Task ExecuteTaskAsync(CancellationToken cancellationToken)
	{
		Logger.LogInformation("{Name} is executing scheduled task", Name);
		await RunJobAsync(cancellationToken);
		_timer.Change(_timeSpan, TimeSpan.FromMilliseconds(1));
	}

	/// <summary>
	/// This method is called when the <see cref="IHostedService"/> starts. The implementation should return a task 
	/// </summary>
	/// <param name="cancellationToken">Triggered when <see cref="IHostedService.StopAsync(CancellationToken)"/> is called.</param>
	/// <returns>A <see cref="Task"/> that represents the long running operations.</returns>
	protected abstract Task RunJobAsync(CancellationToken cancellationToken);

	public virtual async Task StopAsync(CancellationToken cancellationToken)
	{
		Logger.LogInformation("{Name} is stopping", Name);
		_timer?.Change(Timeout.Infinite, 0);

		// Stop called without start
		if (_executingTask == null)
		{
			return;
		}

		try
		{
			// Signal cancellation to the executing method
			_cancellationTokenSource.Cancel();
		}
		finally
		{
			// Wait until the task completes or the stop token triggers
			await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
		}
	}

	public void Dispose()
	{
		_cancellationTokenSource.Cancel();
		_timer?.Dispose();
	}
}