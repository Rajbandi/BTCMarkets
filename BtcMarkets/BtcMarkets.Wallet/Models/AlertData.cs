using System;
using System.Collections.Generic;
using System.Text;

namespace BtcMarkets.Wallet.Models
{
    public class AlertData
    {
        public string Title { get; set; } = "Message";

        public string Message { get; set; }

        public string OkText { get; set; } = "OK";

        public Action action { get; set; } 
        
    }
}
