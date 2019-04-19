using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Api.Contracts
{
    public class BaseObject
    {
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    [DataContract]
    public class MarketTick :BaseObject
    {

        [DataMember(Name = "bestBid")]
        public double BestBid { get; set; }

        [DataMember(Name = "bestAsk")]
        public double BestAsk { get; set; }

        [DataMember(Name = "lastPrice")]
        public double LastPrice { get; set; }

        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [DataMember(Name = "instrument")]
        public string Instrument { get; set; }

        [DataMember(Name = "timestamp")]
        public int Timestamp { get; set; }

        [DataMember(Name = "volume24h")]
        public double Volume24h { get; set; }

       
    }

}
