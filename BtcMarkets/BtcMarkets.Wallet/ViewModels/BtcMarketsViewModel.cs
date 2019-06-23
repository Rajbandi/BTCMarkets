using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using BtcMarkets.Wallet.Helpers;
using BtcMarkets.Wallet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace BtcMarkets.Wallet.ViewModels
{
    public class BtcMarketsViewModel : BaseMarketViewModel
    {
        public BtcMarketsViewModel() :base("BtcMarkets")
        {
          
           
        }

     

        public override ObservableCollection<Market> GetMarkets()
        {
            return new ObservableCollection<Market>(AppData.Current.BtcMarkets);
        }

    }
}
