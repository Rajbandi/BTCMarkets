using BtcMarkets.Wallet.Models;
using BtcMarkets.Wallet.ViewModels;

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
    }
}