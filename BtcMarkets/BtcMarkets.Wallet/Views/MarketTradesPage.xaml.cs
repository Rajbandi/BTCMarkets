using BtcMarkets.Wallet.Models;
using BtcMarkets.Wallet.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BtcMarkets.Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MarketTradesPage : ContentPage
    {
        public MarketTradesPage() : this(null)
        {
           
        }
        public MarketTradesPage(Market market)
        {
            InitializeComponent();

            BindingContext = ViewModel = new TradesViewModel(market);
        }
        public IItemsLayout MarketsLayout => ListItemsLayout.HorizontalList;

        public TradesViewModel ViewModel { get; private set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.InitMarket();
        }

        private void Menu_Tapped(object sender, EventArgs e)
        {
            var shell = (AppShell)Application.Current.MainPage;
            if (shell != null)
            {
                shell.FlyoutIsPresented = !shell.FlyoutIsPresented;
            }
        }
    }
}