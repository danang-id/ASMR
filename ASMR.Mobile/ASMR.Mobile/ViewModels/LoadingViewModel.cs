using ASMR.Mobile.Views;
using Xamarin.Forms;

namespace ASMR.Mobile.ViewModels
{
	public class LoadingViewModel : BaseViewModel
	{
		public LoadingViewModel()
		{
			Title = "Loading";
		}
		
		public static async void Init()
		{
			await ApplicationState.UpdateUserData();
			if (ApplicationState.IsAuthenticated)
			{
				await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
			}
			else
			{
				await Shell.Current.GoToAsync($"//{nameof(SignInPage)}");
			}
		}
	}
}