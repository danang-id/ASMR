using ASMR.Mobile.Views;
using System.Diagnostics;
using System.Text.Json;
using System.Windows.Input;
using ASMR.Common.Constants;
using ASMR.Mobile.Common.Abstractions;
using ASMR.Mobile.Common.Events;
using Xamarin.Forms;

namespace ASMR.Mobile
{
    public partial class AppShell
    {
        private static IApplicationState ApplicationState =>
            DependencyService.Get<IApplicationState>();

        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            ApplicationState.AuthenticationChangedEvent += OnAuthenticationChanged;
            BindingContext = this;
        }
        
        public ICommand SignOutCommand => new Command(ExecuteSignOut);
        
        private void OnAuthenticationChanged(object sender, AuthenticationEventArgs e)
        {
            var user = JsonSerializer.Serialize(e.User, JsonConstants.DefaultJsonSerializerOptions);
            Debug.WriteLine($"Auth State: {e.PreviousState} => {e.State}", GetType().Name);
            Debug.WriteLine($"Auth User: {user}", GetType().Name);
        }

        private async void ExecuteSignOut()
        {
            var result = await ApplicationState.SignOutAsync();
            if (result.IsSuccess)
            {
                await Current.GoToAsync($"//{nameof(SignInPage)}");
            }
            
            Debug.WriteLine(result, GetType().Name);
        }
    }
}
