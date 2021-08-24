using System.Windows.Input;
using Xamarin.Forms;

namespace ASMR.Mobile.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            Name = ApplicationState.User.FirstName ?? "";
            
            DataUpdateCommand = new Command(ExecuteDataUpdate);
        }
        
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public ICommand DataUpdateCommand { get; }

        private async void ExecuteDataUpdate()
        {
            IsBusy = true;
            Name = "Updating...";

            try
            {
                var result = await ApplicationState.UpdateUserData();
                if (result.IsSuccess)
                {
                    Name = ApplicationState.User.FirstName ?? "";
                    return;
                }

                Name = "Not Authenticated";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}