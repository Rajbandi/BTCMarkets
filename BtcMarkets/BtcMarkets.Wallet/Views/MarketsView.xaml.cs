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
    public partial class MarketsView : ContentView
    {
      
        public MarketsView()
        {
            InitializeComponent();
            
        }

        public BaseMarketViewModel ViewModel => (BaseMarketViewModel)this.BindingContext;
        private void MarketsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var market = (Market)marketsListView.SelectedItem;
            if (market != null)
            {
                var shell = (Shell)Application.Current.MainPage;

                if (market != null)
                {

                    AppData.Current.Market = market;
                    var detailsPage = new MarketDetailPage();
                    detailsPage.InputTransparent = false;
                    detailsPage.BackgroundColor = Color.White;
                    Navigation.PushAsync(detailsPage);
                    marketsListView.SelectedItem = null;


                }
            }

        }

        //protected override void OnSizeAllocated(double width, double height)
        //{
        //    base.OnSizeAllocated(width, height);

        //    if (width > 0 && height > 0)
        //    {
        //        var height1 = marketsListView.Height;
        //        var deviceHeight = Device.Info.ScaledScreenSize.Height;
        //        if(height1 == deviceHeight)
        //        {
        //            marketsListView.HeightRequest = deviceHeight - 200;
        //        }
        //    }
        //}


        private void SearchCoin_TextChanged(object sender, TextChangedEventArgs e)
        {
           // ViewModel.SearchMarkets(e.NewTextValue);
        }

        private void SearchCoin_SearchButtonPressed(object sender, EventArgs e)
        {
            //ViewModel.SearchMarkets(SearchCoin.Text);
        }

        private void MarketsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var market = (Market)marketsListView.SelectedItem;
            if (market != null)
            {
                var shell = (Shell)Application.Current.MainPage;

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

        private void Grid_BindingContextChanged(object sender, EventArgs e)
        {

        }

        private void Menu_Tapped(object sender, EventArgs e)
        {
            var shell = (AppShell)Application.Current.MainPage;
            if(shell != null)
            {
                shell.FlyoutIsPresented = !shell.FlyoutIsPresented;
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            
        }

        private void MarketsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var market = (Market)e.Item;
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