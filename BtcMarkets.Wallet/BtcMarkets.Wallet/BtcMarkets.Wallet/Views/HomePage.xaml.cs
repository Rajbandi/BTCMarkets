using BtcMarkets.Wallet.Helpers;
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
    public partial class HomePage : ContentPage
    {
        public HomePageViewModel ViewModel { get; private set; }
        public HomePage()
        {
            InitializeComponent();

            BindingContext = ViewModel = new HomePageViewModel();

            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            AppHelper.TrackEvent(AppTrackEvents.Home);
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            var app = ((App)Application.Current);

            if(app.CurrentTheme == ThemeList.LightTheme)
            {
                app.ChangeTheme(ThemeList.DarkTheme);
            }
            else
                app.ChangeTheme(ThemeList.LightTheme);

        }
    }
}