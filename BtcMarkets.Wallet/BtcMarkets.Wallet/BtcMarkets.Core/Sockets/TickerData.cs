using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using BtcMarkets.Core.Helpers;
using Newtonsoft.Json;

namespace BtcMarkets.Core.Sockets
{
    [JsonObject]
    [DataContract]
    public class TickerData : SocketData
    {

        [JsonProperty("volume24h")]
        [DataMember(Name= "volume24h")]
        public long Volume24h { get; set; }

        [JsonProperty("bestBid")]
        [DataMember(Name= "bestBid")]
        public long BestBid { get; set; }

        [JsonProperty("bestAsk")]
        [DataMember(Name= "bestAsk")]
        public long BestAsk { get; set; }

        [JsonProperty("lastPrice")]
        [DataMember(Name= "lastPrice")]
        public long LastPrice { get; set; }

        [JsonProperty("volume24hDouble")]
        [DataMember(Name = "volume24hDouble")]
        public double Volume24hDouble => ApiHelper.ToDoubleValue(Volume24h);

        [JsonProperty("bestBidDouble")]
        [DataMember(Name = "bestBidDouble")]
        public double BestBidDouble => ApiHelper.ToDoubleValue(BestBid);

        [JsonProperty("bestAskDouble")]
        [DataMember(Name = "bestAskDouble")]
        public double BestAskDouble => ApiHelper.ToDoubleValue(BestAsk);

        [JsonProperty("lastPriceDouble")]
        [DataMember(Name = "lastPriceDouble")]
        public double LastPriceDouble => ApiHelper.ToDoubleValue(LastPrice);

        [JsonProperty("timestamp")]
        [DataMember(Name= "timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("currency")]
        [DataMember(Name= "currency")]
        public string Currency { get; set; }

        [JsonProperty("instrument")]
        [DataMember(Name= "instrument")]
        public string Instrument { get; set; }
    }

}
