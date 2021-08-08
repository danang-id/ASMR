using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace ASMR.Mobile.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string _itemId;
        private string _text;
        private string _description;
        public string Id { get; set; }

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string ItemId
        {
            get
            {
                return _itemId;
            }
            set
            {
                _itemId = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                Id = item.Id;
                Text = item.Text;
                Description = item.Description;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
