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
        public MarketsViewModel ViewModel => (MarketsViewModel)BindingContext;

        public static readonly BindableProperty MarketsProperty = BindableProperty.Create(nameof(Markets), typeof(List<Market>), typeof(MarketsView), default(string), BindingMode.OneWay);
        public List<Market> Markets
        {
            get
            {
                return (List<Market>)GetValue(MarketsProperty);
            }
            set
            {
                SetValue(MarketsProperty, value);
            }
        }

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
            ViewModel.SearchMarkets(e.NewTextValue);
        }

        private void SearchCoin_SearchButtonPressed(object sender, EventArgs e)
        {
            ViewModel.SearchMarkets(SearchCoin.Text);
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
    }
}