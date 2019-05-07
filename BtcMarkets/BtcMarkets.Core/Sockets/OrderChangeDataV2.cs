using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Sockets
{
    [DataContract]
    public class TradeV2
    {

        [DataMember(Name = "tradeId")]
        public int TradeId { get; set; }

        [DataMember(Name = "price")]
        public string Price { get; set; }

        [DataMember(Name = "volume")]
        public string Volume { get; set; }

        [DataMember(Name = "fee")]
        public string Fee { get; set; }

        [DataMember(Name = "liquidityType")]
        public string LiquidityType { get; set; }
    }

    [DataContract]
    public class OrderChangeDataV2
    {

        [DataMember(Name = "orderId")]
        public int OrderId { get; set; }

        [DataMember(Name = "marketId")]
        public string MarketId { get; set; }

        [DataMember(Name = "side")]
        public string Side { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "openVolume")]
        public string OpenVolume { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "triggerStatus")]
        public string TriggerStatus { get; set; }

        [DataMember(Name = "trades")]
        public IList<TradeV2> Trades { get; set; }

        [DataMember(Name = "timestamp")]
        public DateTime Timestamp { get; set; }

        [DataMember(Name = "messageType")]
        public string MessageType { get; set; }
    }

}
