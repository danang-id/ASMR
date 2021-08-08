using ASMR.Mobile.ViewModels;
using Xamarin.Forms.Xaml;

namespace ASMR.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }
    }
}