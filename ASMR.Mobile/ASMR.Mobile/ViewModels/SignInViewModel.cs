using System.Linq;
using ASMR.Mobile.Views;
using Xamarin.Forms;

namespace ASMR.Mobile.ViewModels
{
    public class SignInViewModel : BaseViewModel
    {
        public Command SignInCommand { get; }
        public SignInViewModel()
        {
            SignInCommand = new Command(ExecuteSignIn);
        }

        private string _signInStatus = string.Empty;
        public string SignInStatus
        {
            get => _signInStatus;
            set => SetProperty(ref _signInStatus, value);
        }
        
        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }
        
        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private async void ExecuteSignIn()
        {
            IsBusy = true;
            SignInStatus = "Please wait...";

            try
            {
                var result = await ApplicationState.SignIn(Username, Password);
                if (result.IsSuccess)
                {
                    SignInStatus = string.Empty;
                    Password = string.Empty;
                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                    return;
                }

                if (result.Errors is null || !result.Errors.Any())
                {
                    SignInStatus = "Failed with no reason.";
                    return;
                }

                foreach (var error in result.Errors)
                {
                    Logging.LogError(error.Reason);
                    SignInStatus = error.Reason;
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
