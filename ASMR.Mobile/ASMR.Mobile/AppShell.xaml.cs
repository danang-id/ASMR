using ASMR.Mobile.Services.Abstraction;
using ASMR.Mobile.Services.Events;
using ASMR.Mobile.Views;
using System.Diagnostics;
using System.Text.Json;
using System.Windows.Input;
using ASMR.Common.Constants;
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
            Debug.WriteLine($"[{GetType().Name}] Auth State: {e.PreviousState} => {e.State}");
            Debug.WriteLine($"[{GetType().Name}] Auth User: {user}");
        }

        private async void ExecuteSignOut()
        {
            var result = await ApplicationState.SignOut();
            if (result.IsSuccess)
            {
                await Current.GoToAsync($"//{nameof(SignInPage)}");
            }
            
            Debug.WriteLine(result, GetType().Name);
        }
    }
}
