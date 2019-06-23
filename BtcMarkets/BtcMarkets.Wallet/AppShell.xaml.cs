using BtcMarkets.Wallet.Helpers;
using BtcMarkets.Wallet.Models;
using BtcMarkets.Wallet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace BtcMarkets.Wallet
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public MainPageViewModel ViewModel { get; private set; }
        public AppShell()
        {
            InitializeComponent();
            BindingContext = ViewModel = new MainPageViewModel();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Device.RuntimePlatform == Device.iOS)
            {
                var safeInsets = On<Xamarin.Forms.PlatformConfiguration.iOS>().SafeAreaInsets();
                safeInsets.Left = 24;
                this.Padding = safeInsets;
            }

            AppHelper.TrackEvent(AppTrackEvents.MainPage);
        }


        private static bool isFirstTime = true;
        private void Shell_Navigating(object sender, ShellNavigatingEventArgs e)
        {
            var current = e.Current;
            if (current != null)
            {
                var currentLocation = current.Location.PathAndQuery.ToLower();
                var path = e.Target.Location.PathAndQuery.ToLower();
                if (!currentLocation.Contains("/account") && path.Contains("/account"))
                {
                    if (!AppData.Current.IsAccountSetup)
                    {
                        AppHelper.ShowAlert("Account features requires valid api crendentials. Use settings to setup.");
                    }
                }
              
              
            }
           
        }

        private void MainPageRef_Navigated(object sender, ShellNavigatedEventArgs e)
        {


        }
    }
}
