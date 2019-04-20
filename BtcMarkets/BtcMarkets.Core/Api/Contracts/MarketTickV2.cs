using BtcMarkets.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Api.Contracts
{
    [DataContract]
    public class MarketTickV2 :BaseObject
    {
        [DataMember(Name = "timestamp")]
        public long Timestamp { get; set; }

        [DataMember(Name = "open")]
        public long Open { get; set; }

        [DataMember(Name = "high")]
        public long High { get; set; }

        [DataMember(Name = "low")]
        public long Low { get; set; }

        [DataMember(Name = "close")]
        public long Close { get; set; }

        [DataMember(Name = "volume")]
        public long Volume { get; set; }
    }

}
