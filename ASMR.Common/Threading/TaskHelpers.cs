using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ASMR.Common.Threading;

public static class TaskHelpers
{
	/// <summary>
	/// Executes an async Task method which has a void return value synchronously
	/// </summary>
	/// <param name="task">Task method to execute</param>
	public static void RunSync(Func<Task> task)
	{
		var oldContext = SynchronizationContext.Current;
		var syncContent = new ExclusiveSynchronizationContext();
		SynchronizationContext.SetSynchronizationContext(syncContent);

		async void SendOrPostCallback(object _)
		{
			try
			{
				await task();
			}
			catch (Exception e)
			{
				syncContent.InnerException = e;
				throw;
			}
			finally
			{
				syncContent.EndMessageLoop();
			}
		}

		syncContent.Post(SendOrPostCallback, null);
		syncContent.BeginMessageLoop();

		SynchronizationContext.SetSynchronizationContext(oldContext);
	}

	/// <summary>
	/// Executes an async Task method which has a T return type synchronously
	/// </summary>
	/// <typeparam name="T">Return Type</typeparam>
	/// <param name="task">Task method to execute</param>
	/// <returns></returns>
	public static T RunSync<T>(Func<Task<T>> task)
	{
		var oldContext = SynchronizationContext.Current;
		var syncContext = new ExclusiveSynchronizationContext();
		SynchronizationContext.SetSynchronizationContext(syncContext);
		var result = default(T);

		async void SendOrPostCallback(object _)
		{
			try
			{
				result = await task();
			}
			catch (Exception e)
			{
				syncContext.InnerException = e;
				throw;
			}
			finally
			{
				syncContext.EndMessageLoop();
			}
		}

		syncContext.Post(SendOrPostCallback, null);
		syncContext.BeginMessageLoop();
		SynchronizationContext.SetSynchronizationContext(oldContext);
		return result;
	}

	private class ExclusiveSynchronizationContext : SynchronizationContext
	{
		private bool _done;
		public Exception InnerException { get; set; }
		private readonly AutoResetEvent _workItemsWaiting = new(false);

		private readonly Queue<Tuple<SendOrPostCallback, object>> _items = new();

		public override void Send(SendOrPostCallback d, object state)
		{
			throw new NotSupportedException("Can't send to the same thread.");
		}

		public override void Post(SendOrPostCallback d, object state)
		{
			lock (_items)
			{
				_items.Enqueue(Tuple.Create(d, state));
			}

			_workItemsWaiting.Set();
		}

		public void EndMessageLoop()
		{
			Post(_ => _done = true, null);
		}

		public void BeginMessageLoop()
		{
			while (!_done)
			{
				Tuple<SendOrPostCallback, object> task = null;
				lock (_items)
				{
					if (_items.Count > 0)
					{
						task = _items.Dequeue();
					}
				}

				if (task != null)
				{
					task.Item1(task.Item2);
					if (InnerException != null) // the method threw an exception
					{
						// throw InnerException;
						throw new AggregateException("Error executing asynchronous task.", InnerException);
					}
				}
				else
				{
					_workItemsWaiting.WaitOne();
				}
			}
		}

		public override SynchronizationContext CreateCopy()
		{
			return this;
		}
	}
}