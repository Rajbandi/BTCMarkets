using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using BtcMarkets.Wallet.Helpers;
using BtcMarkets.Wallet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace BtcMarkets.Wallet.ViewModels
{
    public class FavMarketsViewModel : MarketsViewModel
    {
        public FavMarketsViewModel() : base()
        {
            Title = "Favourites";
            Markets = new ObservableCollection<Market>();
            IsSearchBarVisible = false;
            Subscribe();
        }

        private void AppData_FavouritesUpdated(object sender, EventArgs e)
        {
            LoadMarkets();
            OnPropertyChanged(nameof(Markets));
        }

        
        protected override void LoadMarkets()
        {
          //  Markets = new ObservableCollection<Market>(AppData.Current.Favourites);
          if(Markets.Any())
                Markets.Clear();

            var markets = AppData.Current.Favourites;
            foreach(var market in markets)
            {
                if(!market.Starred)
                {
                    market.Starred = true;
                }
                Markets.Add(market);
            }
            base.LoadMarkets();
        }

        public void Subscribe()
        {
            var appData = AppData.Current;

            appData.FavouritesUpdated += AppData_FavouritesUpdated;

        }

        public void Unsubscribe()
        {
            var appData = AppData.Current;

            appData.FavouritesUpdated -= AppData_FavouritesUpdated;

        }

    }

}
