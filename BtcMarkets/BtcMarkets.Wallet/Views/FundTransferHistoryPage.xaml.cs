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
    public partial class FundTransferHistoryPage : ContentPage
    {
        public FundTransferHistoryPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new FundTransferHistoryViewModel();
        }

        public FundTransferHistoryViewModel ViewModel { get; private set; }

      
        protected override void OnAppearing()
        {
            base.OnAppearing();
          
        }
    }
}