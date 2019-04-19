using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using BtcMarkets.Core.Helpers;
using Newtonsoft.Json;

namespace BtcMarkets.Core.Sockets
{
    [JsonObject]
    [DataContract]    
    public class OrderBookData : SocketData
    {

        [JsonProperty("currency")]
        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [JsonProperty("instrument")]
        [DataMember(Name = "instrument")]
        public string Instrument { get; set; }

        [JsonProperty("timestamp")]
        [DataMember(Name = "timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("marketId")]
        [DataMember(Name = "marketId")]
        public int MarketId { get; set; }

        [JsonProperty("snapshotId")]
        [DataMember(Name = "snapshotId")]
        public long SnapshotId { get; set; }

        [JsonProperty("bids")]
        [DataMember(Name = "bids")]
        public IList<long[]> Bids { get; set; }

        [JsonProperty("bidsDouble")]
        [DataMember(Name = "bidsDouble")]
        public IList<double[]> BidsDouble
        {
            get
            {
                var list = new List<double[]>();
                foreach (var bid in Bids)
                {
                    var longArray = bid;
                    var doubleArray = new List<double>();
                    foreach (var l in longArray.Reverse().Skip(1).Reverse())
                    {
                        doubleArray.Add(ApiHelper.ToDoubleValue(l));
                    }

                    list.Add(doubleArray.ToArray());
                }

                return list;
            }

        }

        [JsonProperty("asks")]
        [DataMember(Name = "asks")]
        public IList<long[]> Asks { get; set; }

        [JsonProperty("asksDouble")]
        [DataMember(Name = "asksDouble")]
        public IList<double[]> AsksDouble
        {
            get
            {
                var list = new List<double[]>();
                foreach (var ask in Asks)
                {
                    var longArray = ask;
                    var doubleArray = new List<double>();
                    foreach (var l in longArray.Reverse().Skip(1).Reverse())
                    {
                        doubleArray.Add(ApiHelper.ToDoubleValue(l));
                    }
                    
                    list.Add(doubleArray.ToArray());
                }

                return list;
            }

        }
    }

}
