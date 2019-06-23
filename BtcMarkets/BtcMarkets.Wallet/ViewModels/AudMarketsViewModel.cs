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
    public class AudMarketsViewModel : BaseMarketViewModel
    {
       
        public AudMarketsViewModel() : base("AudMarkets")
        {
           
           
        }

        public override ObservableCollection<Market> GetMarkets()
        {
            return new ObservableCollection<Market>(AppData.Current.AudMarkets);
        }

        //    protected override void LoadMarkets()
        //    {
        //        //Markets = new ObservableCollection<Market>(AppData.Current.AudMarkets);
        //        //if (Markets.Any())
        //        //    Markets.Clear();

        //        //foreach (var market in AppData.Current.AudMarkets)
        //        //{
        //        //    Markets.Add(market);
        //        //}
        //        //base.LoadMarkets();
        //    }

        //    public override void SearchMarkets(string coin)
        //    {
        //        //if(string.IsNullOrWhiteSpace(coin))
        //        //{
        //        //    LoadMarkets();
        //        //}
        //        //else
        //        //{
        //        //    if (Markets.Any())
        //        //        Markets.Clear();
        //        //    var txt = coin.ToLower();
        //        //    var markets = AppData.Current.AudMarkets.Where(x => x.Instrument.ToLower().Contains(txt) || x.Name.ToLower().Contains(txt));
        //        //    foreach (var market in markets)
        //        //    {
        //        //        Markets.Add(market);
        //        //    }

        //        //}
        //        //OnPropertyChanged(nameof(Markets));
        //    }

    }

}
