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
    public class BtcMarketsViewModel : MarketsViewModel
    {
        public BtcMarketsViewModel() :base()
        {
            Title = "BTC Markets";
            _btcHoldings = true;
            IsSearchBarVisible = false;
           // Subscribe();
        }

        protected override void LoadMarkets()
        {
            if (Markets.Any())
                Markets.Clear();

            foreach (var market in AppData.Current.BtcMarkets)
            {
                Markets.Add(market);
            }

            base.LoadMarkets();

            //Markets = new ObservableCollection<Market>(AppData.Current.BtcMarkets);
        }

        public override void SearchMarkets(string coin)
        {
            if (string.IsNullOrWhiteSpace(coin))
            {
                LoadMarkets();
            }
            else
            {
                if (Markets.Any())
                    Markets.Clear();
                var txt = coin.ToLower();
                var markets = AppData.Current.AudMarkets.Where(x => x.Instrument.ToLower().Contains(txt) || x.Name.ToLower().Contains(txt));
                foreach (var market in markets)
                {
                    Markets.Add(market);
                }

            }
            OnPropertyChanged(nameof(Markets));
        }
    }
}
