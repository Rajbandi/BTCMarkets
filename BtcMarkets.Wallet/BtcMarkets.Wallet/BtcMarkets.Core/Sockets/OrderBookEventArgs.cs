using System;
using System.Collections.Generic;
using System.Text;

namespace BtcMarkets.Core.Sockets
{
    public class OrderBookEventArgs : SocketEventArgs
    {
        public OrderBookData OrderBook { get; set; }
    }
}
