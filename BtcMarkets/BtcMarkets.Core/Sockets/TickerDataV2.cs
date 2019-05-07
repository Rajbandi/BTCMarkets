using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Sockets
{
    [DataContract]
    public class TickerDataV2
    {

        [DataMember(Name = "marketId")]
        public string MarketId { get; set; }

        [DataMember(Name = "timestamp")]
        public DateTime Timestamp { get; set; }

        [DataMember(Name = "bestBid")]
        public double BestBid { get; set; }

        [DataMember(Name = "bestAsk")]
        public double BestAsk { get; set; }

        [DataMember(Name = "lastPrice")]
        public double LastPrice { get; set; }

        [DataMember(Name = "volume24h")]
        public double Volume24h { get; set; }

        [DataMember(Name = "messageType")]
        public string MessageType { get; set; }
    }

}
