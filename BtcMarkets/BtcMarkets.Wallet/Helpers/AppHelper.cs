using Acr.UserDialogs;
using BtcMarkets.Wallet.Services;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.Toasts;
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

        public static void SetLoaderMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = "Loading";
            }

            AppService.Instance.SetLoaderMessage(message);
        }
        public static void ShowError(string error = "")
        {
            if(string.IsNullOrWhiteSpace(error))
            {
                error = "Something went wrong.";
            }

            AppService.Instance.ShowError(error);
        }

        public static void ShowMessage(string message, bool isLong)
        {
            DependencyService.Get<IMessage>().ShowMessage(message, isLong);

        }
        public static void ShowMessage(string message, long milliSeconds = 3500)
        {
            DependencyService.Get<IMessage>().ShowMessage(message, milliSeconds);
            
        }

        

        public static  void ShowNotification(string title, string message)
        {
            //var toastConfig = new ToastConfig("Toasting...");
            //toastConfig.SetDuration(3000);
            //toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(12, 131, 193));
            
            //UserDialogs.Instance.Toast(toastConfig);
           
            //UserDialogs.Instance.ShowLoading();
            
            //UserDialogs.Instance.Toast(new ToastConfig
            //{
                    
            //})

            //var notificator = DependencyService.Get<IToastNotificator>();

            //var options = new NotificationOptions()
            //{
            //    Title = title,
            //    Description = message,

            //};

            //var result = await notificator.Notify(options);
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
