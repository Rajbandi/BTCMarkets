using System;
using System.Collections.Generic;
using System.Text;

namespace BtcMarkets.Wallet.Models
{
    public class ActiveMarket
    {
        public string Id => $"{Instrument}-{Currency}";
        public string Name { get; set; }

        public string Instrument { get; set; }

        public string Currency { get; set; }

        public string Image { get; set; }

    }
}
