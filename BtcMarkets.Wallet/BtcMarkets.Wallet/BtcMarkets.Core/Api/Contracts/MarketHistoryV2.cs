using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Api.Contracts
{
    [DataContract]
    public class MarketHistoryV2 :BaseObject
    {

        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "paging")]
        public Paging Paging { get; set; }

        [DataMember(Name = "ticks")]
        public IList<MarketTickV2> Ticks { get; set; }

    }
}
