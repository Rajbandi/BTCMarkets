using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Api.Contracts
{
    [DataContract]
    [JsonObject]
    public class Market
    {
        [JsonProperty(PropertyName = "instrument")]
        [DataMember(Name = "instrument")]
        public string Instrument { get; set; }

        [JsonProperty(PropertyName = "currency")]
        [DataMember(Name = "currency")]
        public string Currency { get; set; }

    }
}
