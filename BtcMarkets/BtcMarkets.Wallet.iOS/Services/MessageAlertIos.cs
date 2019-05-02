using BtcMarkets.Wallet.iOS.Services;
using BtcMarkets.Wallet.Services;
using Foundation;
using UIKit;
[assembly: Xamarin.Forms.Dependency(typeof(MessageAlertIos))]
namespace BtcMarkets.Wallet.iOS.Services
{
    public class MessageAlertIos : IMessage
    {
        const double LONG_DELAY = 3.5;
        const double SHORT_DELAY = 2.0;

        NSTimer alertDelay;
        UIAlertController alert;

        public void ShowMessage(string message, bool longAlert = false)
        {
            if(longAlert)
            {
                ShowAlert(message, LONG_DELAY);
            }
            else
            {
                ShowAlert(message, SHORT_DELAY);
            }
        }
        void ShowAlert(string message, double seconds)
        {
            alertDelay = NSTimer.CreateScheduledTimer(seconds, (obj) =>
            {
                dismissMessage();
            });
            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }

        void dismissMessage()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, null);
            }
            if (alertDelay != null)
            {
                alertDelay.Dispose();
            }
        }

        public void ShowMessage(string message, long seconds = 3500)
        {
            if (seconds <= 0)
                seconds = 3500;

            ShowAlert(message, seconds/1000);
        }
    }
}