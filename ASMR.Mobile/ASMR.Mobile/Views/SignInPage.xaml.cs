using ASMR.Mobile.ViewModels;

namespace ASMR.Mobile.Views
{
    public partial class SignInPage
    {
        public SignInPage()
        {
            InitializeComponent();
            BindingContext = new SignInViewModel();
        }
    }
}