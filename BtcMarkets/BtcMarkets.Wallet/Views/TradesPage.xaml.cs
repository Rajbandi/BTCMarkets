using BtcMarkets.Wallet.Helpers;
using BtcMarkets.Wallet.Models;
using BtcMarkets.Wallet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BtcMarkets.Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TradesPage : ContentPage
    {

        public TradesViewModel ViewModel { get; private set; }
        public TradesPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new TradesViewModel();
 
           


        }

        public IItemsLayout MarketsLayout => ListItemsLayout.HorizontalList;
        protected override void OnAppearing()
        {
            base.OnAppearing();

          

            AppHelper.TrackEvent(AppTrackEvents.Trades);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();


        }
        private void MarketPairs_SelectedIndexChanged(object sender, EventArgs e)
        {
          

        }
    }
}