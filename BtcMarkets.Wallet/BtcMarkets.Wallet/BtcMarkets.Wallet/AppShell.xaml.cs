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

        
        private bool isFirstTime = true;
        private void Shell_Navigating(object sender, ShellNavigatingEventArgs e)
        {
            var current = e.Current;
            if (current != null && current.Location.PathAndQuery.ToLower().Contains("/markets/"))
            {

            }

            // var data = AppData.Current;
            //    var current = e.Current;
            //    if (current != null && current.Location.PathAndQuery.ToLower().Contains("/markets/"))
            //    {
            //        return;
            //    }
            //    var path = e.Target.Location.PathAndQuery.ToLower();

            //    if (e.CanCancel && isFirstTime && path.Contains("/markets/"))
            //    {

            //        isFirstTime = false;

            //        try
            //        {
            //            if (!data.Favourites.Any())
            //            {
            //                if (path.Contains("/markets/fav"))
            //                {
            //                    e.Cancel();
            //                    var location = e.Target.Location.OriginalString.Replace("/Fav/", "/Aud/");
            //                    var state = new ShellNavigationState(location);
            //                    GoToAsync(state);
            //                }
            //            }
            //            else
            //            {
            //                if (!path.Contains("/markets/fav"))
            //                {
            //                    e.Cancel();

            //                    var pathString = e.Target.Location.OriginalString;
            //                    if (pathString.EndsWith("/"))
            //                    {
            //                        pathString = pathString.Substring(0, pathString.LastIndexOf('/'));
            //                    }

            //                    var location = pathString.Substring(0, pathString.LastIndexOf('/')) + "/Fav/";

            //                    var state = new ShellNavigationState(location);
            //                    GoToAsync(state);
            //                }
            //            }
            //        }
            //        catch(Exception ex)
            //        {

            //        }
            //    }

        }

        private void MainPageRef_Navigated(object sender, ShellNavigatedEventArgs e)
        {
            
            
        }
    }
}
