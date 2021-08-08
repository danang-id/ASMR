using ASMR.Mobile.ViewModels;

namespace ASMR.Mobile.Views
{
    public partial class ItemDetailPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}