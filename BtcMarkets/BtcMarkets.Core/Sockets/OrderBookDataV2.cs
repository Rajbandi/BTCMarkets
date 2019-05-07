using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Sockets
{
    [DataContract]
    public class OrderBookDataV2
    {

        [DataMember(Name = "marketId")]
        public string MarketId { get; set; }

        [DataMember(Name = "timestamp")]
        public DateTime Timestamp { get; set; }

        [DataMember(Name = "bids")]
        public IList<IList<string>> Bids { get; set; }

        [DataMember(Name = "asks")]
        public IList<IList<string>> Asks { get; set; }

        [DataMember(Name = "messageType")]
        public string MessageType { get; set; }
    }
}
