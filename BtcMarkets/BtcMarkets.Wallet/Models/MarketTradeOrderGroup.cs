using System;
using System.Collections.Generic;
using System.Text;

namespace BtcMarkets.Wallet.Models
{
    public class MarketTradeOrderGroup : BaseBindableObject
    {
        public string Market { get; set; }

        public List<MarketTradeOrder> Orders { get; set; }

        public MarketTradeOrderGroup()
        {
            Orders = new List<MarketTradeOrder>();
        }
    }
}
