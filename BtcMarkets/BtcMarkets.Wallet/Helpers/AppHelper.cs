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

            var color = GetWebColor("ErrorMessageColor");

            AppService.Instance.ShowError(error, color);
        }

        public static void ShowSuccess(string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = "Success";
            }

            var color = GetWebColor("SuccessMessageColor");

            AppService.Instance.ShowError(message, color);
        }

        public static void ShowMessage(string message)
        {
          
            var color = GetWebColor("MessageColor");
            AppService.Instance.ShowMessage(message, color);
        }

        public static void ShowAlert(string message)
        {
            AppService.Instance.ShowAlert(new Models.AlertData
            {
                Message = message
            });
        }
        public static double CalculateChange(double previous, double current)
        {
            if (previous == 0)
                return 0;

            var change = current - previous;
            return (double)change / previous;
        }

        public static string DoubleToPercentageString(double d)
        {
            return (Math.Round(d, 2) * 100).ToString("0.00") + "%";
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
                if (currency == Constants.Aud)
                str = $"{value:0.00}";
            else
                str = $"{value:0.00000}";

            return str;
        }

        public static string FormatNumberWithSymbol(double value, string currency = "")
        {
            string str;
            if (currency == Constants.Btc)
            {
                str = $"{Constants.BtcSymbol}{value:0.00000000}";
            }
            else
                str = $"{Constants.AudSymbol}{value:0.00}";

            return str;
        }
        public static ImageSource GetMarketImage(string code)
        {
            ImageSource source = null;

            var data = AppData.Current;
            var images = data.MarketImages;
            if (!string.IsNullOrWhiteSpace(code))
            {
                if (images != null && images.ContainsKey(code))
                {
                    ImageSource image;
                    images.TryGetValue(code, out image);
                    source = image;
                }
            }

            return source;
        }
        public static Color GetColor(string style)
        {
            Color color = Color.Default;

            try
            {
                var styleColor = (Color)Application.Current.Resources[style];
                if (styleColor != null)
                {
                    color = styleColor;
                }

            }
            catch (Exception)
            {

            }

            return color;
        }
        public static string GetWebColor(string style)
        {
            return GetColor(style).ToHexWeb() ?? "";
        }

    }
}
