using ASMR.Mobile.Models;
using ASMR.Mobile.ViewModels;

namespace ASMR.Mobile.Views
{
    public partial class NewItemPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}