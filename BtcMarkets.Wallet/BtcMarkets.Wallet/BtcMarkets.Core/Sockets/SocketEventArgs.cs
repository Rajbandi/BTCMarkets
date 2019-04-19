using System;
using System.Collections.Generic;
using System.Text;

namespace BtcMarkets.Core.Sockets
{
    public class SocketEventArgs : EventArgs
    {
        public string Error { get; set; }

        public string Data { get; set; }
    }

}
