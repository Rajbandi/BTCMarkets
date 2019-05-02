using BtcMarkets.Core.Api.Contracts;
using BtcMarkets.Wallet.Helpers;
using BtcMarkets.Wallet.Models;
using BtcMarkets.Wallet.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BtcMarkets.Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BtcMarketsPage : ContentPage
    {
        public readonly BtcMarketsViewModel ViewModel;

        public BtcMarketsPage()
        {
          
          
            InitializeComponent();
            BindingContext = ViewModel = new BtcMarketsViewModel();
          //        ViewModel.RefreshMarkets();
          
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            AppHelper.TrackEvent(AppTrackEvents.BtcMarkets);

        }

    }
}