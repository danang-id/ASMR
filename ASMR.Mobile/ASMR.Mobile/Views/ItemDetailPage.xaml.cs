using ASMR.Mobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace ASMR.Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}