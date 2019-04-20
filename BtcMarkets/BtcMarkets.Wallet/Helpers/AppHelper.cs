using BtcMarkets.Wallet.Services;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Helpers
{
    public static class AppHelper
    {
        public static void ShowLoading(string message = "")
        {
            var service = DependencyService.Get<ILodingPageService>();
            if (service != null)
            {
                service.ShowLoadingPage();
            }
        }

        public static void HideLoading()
        {
            var service = DependencyService.Get<ILodingPageService>();
            if (service != null)
            {
                service.HideLoadingPage();
            }
        }

        public static void TrackError(Exception ex, Dictionary<string, string> values = null)
        {
            if (values != null)
                Crashes.TrackError(ex, values);
            else
                Crashes.TrackError(ex);
        }

        public static void TrackEvent(string eventStr)
        {
            if (!string.IsNullOrWhiteSpace(eventStr))
                Analytics.TrackEvent(eventStr);
        }


        public static string FormatNumber(double value, string currency = "")
        {
            string str;
            if (currency == Constants.Btc)
            {
                str = $"{value:0.00000000}";
            }
            else
                str = $"{value:0.00}";

            return str;
        }

    }
}
