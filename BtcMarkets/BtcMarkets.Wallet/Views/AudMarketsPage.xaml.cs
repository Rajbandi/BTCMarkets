using BtcMarkets.Wallet.Helpers;
using BtcMarkets.Wallet.Models;
using BtcMarkets.Wallet.ViewModels;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BtcMarkets.Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AudMarketsPage : ContentPage
    {
        public readonly AudMarketsViewModel ViewModel;

        public AudMarketsPage()
        {
            InitializeComponent();

            BindingContext = ViewModel = new AudMarketsViewModel();

        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            AppHelper.TrackEvent(AppTrackEvents.AudMarkets);

            CheckFavourites();
        }

        private static bool isFirst = true;
        public async void CheckFavourites()
        {
            if(!isFirst)
            {
                return;
            }
            isFirst = false;
            var shell = (Shell)Application.Current.MainPage;
            if (shell != null)
            {
                var data = AppData.Current;
                if (data.Favourites != null && data.Favourites.Any())
                {
                    ViewModel.IsBusy = true;
                    var item = shell.CurrentItem;


                    var pathString = shell.CurrentState.Location.OriginalString;
                    if (pathString.EndsWith("/"))
                    {
                        pathString = pathString.Substring(0, pathString.LastIndexOf('/'));
                    }

                    var location = pathString.Substring(0, pathString.LastIndexOf('/')) + "/Fav/";

                    var state = new ShellNavigationState(location);
                    await shell.GoToAsync(state);
                    ViewModel.IsBusy = false;
                }
            }
        }
    }
}