using ASMR.Mobile.ViewModels;

namespace ASMR.Mobile.Views
{
	public partial class LoadingPage
	{
		public LoadingPage()
		{
			InitializeComponent();
			BindingContext = new LoadingViewModel();
		}
		
		protected override void OnAppearing()
		{
			base.OnAppearing();
			LoadingViewModel.Init();
		}
	}
}