using System;
using System.Collections.Generic;
using System.Text;

namespace BtcMarkets.Wallet.Models
{
    public class MarketFavourite
    {
        public MarketFavourite()
        {

        }

        public MarketFavourite(string instrument, string currency)
        {
            Instrument = instrument;
            Currency = currency;
        }

        public string Instrument { get; set; }

        public string Currency { get; set; }
    }
}
