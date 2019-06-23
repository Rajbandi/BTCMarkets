using BtcMarkets.Wallet.Helpers;
using BtcMarkets.Wallet.Models;
using BtcMarkets.Wallet.ViewModels;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BtcMarkets.Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavMarketsPage : ContentPage
    {
        public readonly FavMarketsViewModel ViewModel;

        public FavMarketsPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new FavMarketsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            AppHelper.TrackEvent(AppTrackEvents.Favourites);
            //  CheckFavourites();


           Refresh();

        }

        private async void Refresh()
        {
            if (isFirst)
            {
                ViewModel.IsBusy = true;
                await ViewModel.RefreshMarkets();
                CheckFavourites();
                ViewModel.IsBusy = false;
            }
        }

        private static bool isFirst = true;
        public async void CheckFavourites()
        {
            if (!isFirst)
                return;

            ViewModel.IsBusy = true;
            isFirst = false;
            var data = AppData.Current;
            if (data.Favourites == null || !data.Favourites.Any())
            {
                var shell = (Shell)Application.Current.MainPage;
                if (shell != null)
                {
                    var item = shell.CurrentItem;


                    var pathString = shell.CurrentState.Location.OriginalString;
                    if (pathString.EndsWith("/"))
                    {
                        pathString = pathString.Substring(0, pathString.LastIndexOf('/'));
                    }

                    var location = pathString.Substring(0, pathString.LastIndexOf('/')) + "/AUD/";

                    var state = new ShellNavigationState(location);
                    await shell.GoToAsync(state);
                }
            }

            ViewModel.IsBusy = false;
        }
    }
}