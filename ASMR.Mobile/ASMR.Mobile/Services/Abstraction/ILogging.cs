using System;
using System.Collections.Generic;
using ASMR.Mobile.Services.Logging;

namespace ASMR.Mobile.Services.Abstraction
{
	public interface ILogging
	{
		public void CaptureEvent(LogLevel level, string message);
		public void CaptureEvent(LogLevel level, string message, IDictionary<string, string> properties);
		public void CaptureException(Exception exception);
		public void CaptureException(Exception exception, IDictionary<string, string> properties);
		
		public void LogError(string message);
		public void LogError(Exception exception, string message);
		public void LogInformation(string message);
		public void LogWarning(string message);
	}
}