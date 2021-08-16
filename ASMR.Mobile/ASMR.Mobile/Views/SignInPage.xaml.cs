using ASMR.Mobile.ViewModels;
using Xamarin.Forms.Xaml;

namespace ASMR.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignInPage
    {
        public SignInPage()
        {
            InitializeComponent();
            BindingContext = new SignInViewModel();
        }
    }
}