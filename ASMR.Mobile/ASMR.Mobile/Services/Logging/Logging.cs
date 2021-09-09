using System;
using System.Collections.Generic;
#if DEBUG
using System.Diagnostics;
#endif
using System.Linq;
using ASMR.Mobile.Services.Abstraction;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(ASMR.Mobile.Services.ApplicationState))]
namespace ASMR.Mobile.Services.Logging
{
	public class Logging : ILogging
	{
		private readonly IApplicationState _applicationState;
 
		public Logging()
		{
			_applicationState = DependencyService.Get<IApplicationState>();
		}

		public void CaptureEvent(LogLevel level, string message)
		{
			CaptureEvent(level, message, new Dictionary<string, string>());
		}
 
		public void CaptureEvent(LogLevel level, string message, IDictionary<string, string> properties)
		{
			if (_applicationState.LogLevel > level || IsOnTestCloud() || IsEmulatorOrSimulator())
			{
				return;
			}

			properties = properties.Concat(GetDefaultProperties()) as Dictionary<string, string>;
			Analytics.TrackEvent($"{level}: {message}", properties);
		}

		public void CaptureException(Exception exception)
		{
			CaptureException(exception, new Dictionary<string, string>());
		}
		
		public void CaptureException(Exception exception, IDictionary<string, string> properties)
		{
			if (IsOnTestCloud() || IsEmulatorOrSimulator())
			{
				return; 
			}
			properties = properties.Concat(GetDefaultProperties()) as Dictionary<string, string>;
			
#if DEBUG
			Debug.WriteLine(exception, GetType().Name);
#endif
			
			Crashes.TrackError(exception, properties);
		}

		public void LogError(string message)
		{
			CaptureEvent(LogLevel.Error, message);
		}

		public void LogError(Exception exception, string message)
		{
			CaptureEvent(LogLevel.Error, message, new Dictionary<string, string>
			{
				{ "ExceptionMessage", exception.Message }
			});
			CaptureException(exception);
		}

		public void LogInformation(string message)
		{
			CaptureEvent(LogLevel.Information, message);
		}

		public void LogWarning(string message)
		{
			CaptureEvent(LogLevel.Warning, message);
		}

		private Dictionary<string, string> GetDefaultProperties()
		{
			return new Dictionary<string, string>
			{
				{ "InstallId", _applicationState.InstallId.ToString() }
			};
		}

		private static bool IsEmulatorOrSimulator()
		{
			return DeviceInfo.DeviceType == DeviceType.Virtual;
		}
 
		private static bool IsOnTestCloud()
		{
			var isInTestCloud = Environment.GetEnvironmentVariable("XAMARIN_TEST_CLOUD");
			return isInTestCloud is "1";
		}
	}
}