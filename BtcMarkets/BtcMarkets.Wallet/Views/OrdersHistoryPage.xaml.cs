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
    public partial class OrdersHistoryPage : ContentPage
    {
        public OrdersHistoryPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new OrdersHistoryViewModel();
        }

        public OrdersHistoryViewModel ViewModel { get; private set; }

        public IItemsLayout MarketsLayout => ListItemsLayout.HorizontalList;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.InitMarket();
        }
    }
}