using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BtcMarkets.Wallet.Services;
using BtcMarkets.Wallet.Droid.Services;

[assembly: Xamarin.Forms.Dependency(typeof(MessagerAndroid))]
namespace BtcMarkets.Wallet.Droid.Services
{
    public class MessagerAndroid : IMessage
    {
        public void ShowMessage(string message, bool longAlert = false)
        {
            if (longAlert)
            {
                var toast = Toast.MakeText(Application.Context, message, ToastLength.Long);
                toast.SetGravity(GravityFlags.Center, 0, 0);
                toast.Show();
            }
            else
            {
                var toast = Toast.MakeText(Application.Context, message, ToastLength.Short);
                toast.SetGravity(GravityFlags.Center, 0, 0);
                toast.Show();

            }
        }

        public void ShowMessage(string message, long milliSeconds = 3000)
        {
            var toast = Toast.MakeText(Application.Context, message, ToastLength.Short);
            toast.SetGravity(GravityFlags.Center, 0, 0);

            toast.Show();

            var timer = new MessageTimer(toast,milliSeconds);
            timer.Start();

        }


    }

    public class MessageTimer : CountDownTimer
    {
        private Toast _toast;

        public MessageTimer(Toast toast, long millisInFuture, long countDownInterval=1000) : base(millisInFuture, countDownInterval)
        {
            _toast = toast;
        }

        public override void OnFinish()
        {
            _toast?.Show();
        }

        public override void OnTick(long millisUntilFinished)
        {
            _toast?.Show();
        }
    }
}