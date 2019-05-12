
using BtcMarkets.Wallet.Helpers;
using BtcMarkets.Wallet.Models;
using BtcMarkets.Wallet.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BtcMarkets.Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MarketsPage : ContentPage
    {
       
        public MarketsPage()
        {
            InitializeComponent();

            BindingContext = ViewModel = new MarketsPageViewModel();
          
        }

        public MarketsPageViewModel ViewModel { get; private set; }

        
        protected override void OnAppearing()
        {
            base.OnAppearing();
         
          
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            try
            {
                base.OnSizeAllocated(width, height);
            }
            catch(Exception)
            {

            }

        }
        protected override void OnChildMeasureInvalidated(object sender, EventArgs e)
        {
            try
            {
                base.OnChildMeasureInvalidated(sender, e);
            }
            catch(Exception)
            {

            }
            
        }


        private void SearchCoin_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void SearchCoin_SearchButtonPressed(object sender, EventArgs e)
        {
           
        }

        private void MarketsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var market = (Market)marketsListView.SelectedItem;
            if (market != null)
            {
                if (market != null)
                {
                    ViewModel.IsBusy = true;
                    AppData.Current.Market = market;
                    var detailsPage = new MarketDetailPage();
                    detailsPage.InputTransparent = false;
                    detailsPage.BackgroundColor = Color.White;
                    Navigation.PushAsync(detailsPage);
                    marketsListView.SelectedItem = null;


                }
            }
        }
    }
}