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
    public class AudMarketsViewModel : MarketsViewModel
    {
       
        public AudMarketsViewModel() : base()
        {
            Title = "AUD Markets";
           
        }

        protected override void LoadMarkets()
        {
            //Markets = new ObservableCollection<Market>(AppData.Current.AudMarkets);
            if (Markets.Any())
                Markets.Clear();

            foreach (var market in AppData.Current.AudMarkets)
            {
                Markets.Add(market);
            }
        }
    }

}
