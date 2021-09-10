using ASMR.Mobile.Common.Abstractions;
using ASMR.Mobile.iOS.Common;
using Foundation;
using UIKit;

// ReSharper disable InconsistentNaming
[assembly: Xamarin.Forms.Dependency(typeof(IOSAlertHandler))]
namespace ASMR.Mobile.iOS.Common
{
	public class IOSAlertHandler : IAlertHandler
	{
		private const double LONG_DELAY = 3.5;
		private const double SHORT_DELAY = 2.0;

		private NSTimer alertDelay;
		private UIAlertController alert;
		
		private void ShowAlert(string message, double seconds)
		{
			alertDelay = NSTimer.CreateScheduledTimer(seconds, _ => DismissMessage());
			alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
			UIApplication.SharedApplication.KeyWindow.RootViewController?
				.PresentViewController(alert, true, null);
		}

		private void DismissMessage()
		{
			alert?.DismissViewController(true, null);
			alertDelay?.Dispose();
		}
		
		public void Display(string message, AlertLength length = AlertLength.Short)
		{
			ShowAlert(message, length switch
			{
				AlertLength.Long => LONG_DELAY,
				AlertLength.Short => SHORT_DELAY,
				_ => LONG_DELAY
			});
		}
	}
}