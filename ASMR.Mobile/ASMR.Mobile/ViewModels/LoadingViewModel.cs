using ASMR.Mobile.Views;
using Xamarin.Forms;

namespace ASMR.Mobile.ViewModels
{
	public class LoadingViewModel : BaseViewModel
	{
		public LoadingViewModel()
		{
			Title = "ASMR";
		}
		
		public static async void Init()
		{
			await ApplicationState.UpdateUserDataAsync();
			if (ApplicationState.IsAuthenticated)
			{
				await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
			}
			else
			{
				await Shell.Current.GoToAsync($"//{nameof(SignInPage)}");
			}
		}
	}
}