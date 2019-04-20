using System;
using System.Collections.Generic;
using System.Text;

namespace BtcMarkets.Core.Sockets
{
    public class TickerEventArgs : SocketEventArgs
    {
        public TickerData Ticker { get; set; }
    }

}
