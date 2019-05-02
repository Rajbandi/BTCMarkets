using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BtcMarkets.Wallet.Models
{
    public class MarketOrderGroup : List<MarketOrderData>
    {
        public string MarketName { get; set; }

        public MarketOrderGroup()
        {
         
        }

        public MarketOrderGroup(string marketName, List<MarketOrderData> data)
        {
            MarketName = marketName;
            if(data != null)
            {
                AddRange(data);
            }
        }
    }
}
