using System;
using Xamarin.Forms;
using System.Linq;
using Xamarin.Forms.Xaml;
using BtcMarkets.Wallet.Services;
using BtcMarkets.Wallet.Views;
using BtcMarkets.Core.Api;
using BtcMarkets.Wallet.Models;
using BtcMarkets.Core;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter;
using System.Collections.Generic;
using BtcMarkets.Wallet.Helpers;


namespace BtcMarkets.Wallet
{
    public enum ThemeList
    {
        LightTheme,
        DarkTheme
    }
    public partial class App : Application
    {

        public ThemeList CurrentTheme { get; set; }
        public static Models.ApiSettings Settings { get; private set; }


        public App()
        {
            InitializeComponent();

            //            DependencyService.Register<MockDataStore>();

            try
            {
                AppCenter.Start("android=6508f0cc-42f1-4498-9094-24fe22e7ebc5;" +
                      "uwp={Your UWP App secret here};" +
                      "ios=37bdd2b9-1bb7-48f6-8a0e-d67cbd3e940a;",
                      typeof(Analytics), typeof(Crashes));
            }
            catch (Exception ex)
            {
                AppHelper.TrackError(ex);
            }

            //Load the assembly
            // Xamarin.Forms.DataGrid.DataGridComponent.Init();
            ChangeTheme();

            
            MainPage = new AppShell();

            // RefreshMarkets();
        }

        /// <summary>
        /// Changes app theme 
        /// </summary>
        /// <param name="theme">ThemeList value</param>
        public void ChangeTheme(ThemeList theme = ThemeList.DarkTheme)
        {
            try
            {
                //Resources.Clear();
                ResourceDictionary themeInstance;
                switch (theme)
                {
                    case ThemeList.LightTheme:
                        themeInstance = (ResourceDictionary)Activator.CreateInstance<Themes.LightTheme>();
                        break;
                    default:
                        themeInstance = (ResourceDictionary)Activator.CreateInstance<Themes.DarkTheme>();
                        break;
                }
                var dict = Resources.MergedDictionaries.FirstOrDefault(x => x.GetType() == themeInstance.GetType());
                if (dict != null)
                {
                    Resources.MergedDictionaries.Remove(dict);
                }
                Resources.MergedDictionaries.Add(themeInstance);
                CurrentTheme = theme;
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                            { "theme", $"{theme}"}
                          };
               AppHelper.TrackError(ex, properties);
            }
        }


        protected override void OnStart()
        {
            // Handle when your app starts

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

    }
}
