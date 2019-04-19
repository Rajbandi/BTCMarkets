using BtcMarkets.Wallet.Helpers;
using BtcMarkets.Wallet.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BtcMarkets.Wallet.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public string HomeIcon { get; set; }
        public string MarketsIcon { get; set; }

        public string TradesIcon { get; set; }

        public string AccountIcon { get; set; }

        public string AboutIcon { get; set; }
        public MainPageViewModel() : base()
        {
            HomeIcon = "home";
            MarketsIcon = "list";
            var data = AppData.Current;

         

        }

      

        private string _logo;
        public string Logo
        {
            get
            {
                return _logo;
            }
            set
            {
                _logo = value;
                OnPropertyChanged(nameof(Logo));
            }
        }
    }
}
