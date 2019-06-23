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
    public class FavMarketsViewModel : BaseMarketViewModel
    {
        public FavMarketsViewModel() : base("Favourites")
        {
            AppData.Current.FavouritesUpdated += Current_FavouritesUpdated;
        }

        private void Current_FavouritesUpdated(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Markets));
        }

        public override ObservableCollection<Market> GetMarkets()
        {
            return new ObservableCollection<Market>(AppData.Current.Favourites);
        }


    }

}
