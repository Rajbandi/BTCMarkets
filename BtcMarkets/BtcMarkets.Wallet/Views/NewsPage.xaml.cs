using BtcMarkets.Wallet.Helpers;
using BtcMarkets.Wallet.Models;
using BtcMarkets.Wallet.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BtcMarkets.Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsPage : ContentPage
    {
        public NewsPageViewModel ViewModel { get; private set; }
        public NewsPage()
        {
            InitializeComponent();

            BindingContext = ViewModel = new NewsPageViewModel();
        }

     
        private async void OpenNewsItem(string uri)
        {

            await Browser.OpenAsync(uri, new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = AppHelper.GetColor("PrimaryColor"),
                PreferredControlColor = AppHelper.GetColor("AccentColor")
            }) ;
          
        }

        private void NewsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ViewModel.IsBusy = true;
            var item = (MarketNewsItem)e.SelectedItem;
            if (item != null)
            {
                NewsList.SelectedItem = null;
                var uri = item.Link;
                OpenNewsItem(uri);
               
            }
            ViewModel.IsBusy = false;
        }
    }
}