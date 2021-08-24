using ASMR.Mobile.Services.Abstraction;
using ASMR.Mobile.Services.Logging;
using Xamarin.Forms;

namespace ASMR.Mobile.Views
{
	public class BaseContentPage : ContentPage
	{
		protected readonly ILogging Logging;
		
		public BaseContentPage()
		{
			Logging = DependencyService.Get<ILogging>();
		}
 
		protected override void OnAppearing()
		{
			base.OnAppearing();
 
			Logging.CaptureEvent(LogLevel.Information, $"{GetType().Name} Appeared");
		}
 
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
 
			Logging.CaptureEvent(LogLevel.Information, $"{GetType().Name} Disappeared");
		}
	}
}