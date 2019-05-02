using BtcMarkets.Wallet.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BtcMarkets.Wallet.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new AboutViewModel();
        }

        public AboutViewModel ViewModel { get; private set; }
    }
}