using System;
using System.Collections.Generic;
using System.Text;

namespace BtcMarkets.Core.Sockets
{
    public class MarketTradeEventArgs : SocketEventArgs
    {
        public MarketTradeData MarketTrade { get; set; }
    }
}
