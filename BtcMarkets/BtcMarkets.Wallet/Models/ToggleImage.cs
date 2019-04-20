using System;
using System.Collections.Generic;
using System.Text;

namespace BtcMarkets.Wallet.Models
{
    public class ToggleImage
    {
        public string FontFamily { get; set; }

        public bool Value { get; set; }

        public string OnImage { get; set; }

        public string OffImage { get; set; }

        public string Color { get; set; }
    }
}
