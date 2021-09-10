using Android.App;
using Android.Widget;
using ASMR.Mobile.Common.Abstractions;
using ASMR.Mobile.Droid.Common;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidAlertHandler))]
namespace ASMR.Mobile.Droid.Common
{
	public class AndroidAlertHandler : IAlertHandler
	{
		public void Display(string message, AlertLength length = AlertLength.Short)
		{
			Toast.MakeText(Application.Context, message, length switch
				{
					AlertLength.Long => ToastLength.Long,
					AlertLength.Short => ToastLength.Short,
					_ => ToastLength.Long
				})
				?.Show();
		}
	}
}