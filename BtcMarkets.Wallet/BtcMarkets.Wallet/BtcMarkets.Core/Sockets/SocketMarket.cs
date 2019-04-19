using System;
using System.Collections.Generic;
using System.Text;

namespace BtcMarkets.Core.Sockets
{
    public class SocketMarket 
    {
        public string Instrument { get; set; }
        public string Currency { get; set; }

        public bool Ticker { get; set; }
        public bool OrderBook { get; set; }
        public bool Trade { get; set; }
    }
}
