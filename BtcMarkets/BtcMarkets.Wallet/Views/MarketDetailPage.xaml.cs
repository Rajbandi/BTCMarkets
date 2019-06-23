using BtcMarkets.Wallet.Controls;
using BtcMarkets.Wallet.Models;
using BtcMarkets.Wallet.Services;
using BtcMarkets.Wallet.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BtcMarkets.Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MarketDetailPage : ContentPage
    {
        public MarketDetailViewModel ViewModel { get; private set; }

        public IItemsLayout ChartPeriodLayout => ListItemsLayout.HorizontalList;
        public MarketDetailPage()
        {
            InitializeComponent();
            var market = AppData.Current.Market;
            BindingContext = ViewModel = new MarketDetailViewModel(market);

        }

       
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ViewModel.InitReport();
                });
                
            });
            
        }

        private void BtnTrades_Clicked(object sender, EventArgs e)
        {
          
            var trades = new MarketTradesPage(ViewModel.Market);

            Navigation.PushAsync(trades);
        }
    }

}