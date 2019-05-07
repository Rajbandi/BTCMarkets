using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Sockets
{
    [DataContract]
    public class MarketTradeDataV2
    {

        [DataMember(Name = "marketId")]
        public string MarketId { get; set; }

        [DataMember(Name = "timestamp")]
        public DateTime Timestamp { get; set; }

        [DataMember(Name = "tradeId")]
        public long TradeId { get; set; }

        [DataMember(Name = "price")]
        public string Price { get; set; }

        [DataMember(Name = "volume")]
        public string Volume { get; set; }

        [DataMember(Name = "messageType")]
        public string MessageType { get; set; }
    }

}
