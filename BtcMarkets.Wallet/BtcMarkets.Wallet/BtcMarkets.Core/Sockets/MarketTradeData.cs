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
    public class MarketTradeData : SocketData
    {

        [JsonProperty("id")]
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [JsonProperty("timestamp")]
        [DataMember(Name = "timestamp")]
        public int Timestamp { get; set; }

        [JsonProperty("marketId")]
        [DataMember(Name = "marketId")]
        public int MarketId { get; set; }

        [JsonProperty("agency")]
        [DataMember(Name = "agency")]
        public string Agency { get; set; }

        [JsonProperty("instrument")]
        [DataMember(Name = "instrument")]
        public string Instrument { get; set; }

        [JsonProperty("currency")]
        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [JsonProperty("trades")]
        [DataMember(Name = "trades")]
        public IList<long[]> Trades { get; set; }

        [JsonProperty("tradesDouble")]
        [DataMember(Name = "tradesDouble")]
        public IList<double[]> TradesDouble
        {
            get
            {
                var list = new List<double[]>();
                foreach (var trade in Trades)
                {
                    var longArray = trade;
                    var doubleArray = new List<double>();
                    foreach (var l in longArray)
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
