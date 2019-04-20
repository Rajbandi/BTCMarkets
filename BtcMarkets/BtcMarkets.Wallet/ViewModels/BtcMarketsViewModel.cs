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


            //Markets = new ObservableCollection<Market>(AppData.Current.BtcMarkets);
        }


    }
}
