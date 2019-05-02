using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Models
{
    public class ToastData
    {

        public string Message { get; set; }

        public string MessageColor { get; set; }

        public Action action { get; set; }

        public TimeSpan Duration { get; set; }

        public string Icon { get; set; }

        public string BackgroundColor { get; set; }

    }
}
