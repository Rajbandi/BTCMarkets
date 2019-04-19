using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Wallet.Models
{
    [DataContract]
    public class MarketTradeOrders
    {
        [DataMember(Name = "timestamp")]
        public long? Timestamp { get; set; }

        [DataMember(Name = "buy")]
        public List<MarketTradeOrder> Buy { get; set; }

        [DataMember(Name = "sell")]
        public List<MarketTradeOrder> Sell { get; set; }

    }
}
