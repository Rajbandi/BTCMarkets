using BtcMarkets.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace BtcMarkets.Wallet.Models
{
    [JsonObject]
    [DataContract]
    public class MarketNewsItem
    {

        [JsonProperty("datepublished")]
        [DataMember(Name = "datepublished")]
        public DateTime DatePublished { get; set; }

        public string DateString => ApiHelper.ToRelativeTime(DatePublished);

        [JsonProperty("title")]
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [JsonProperty("link")]
        [DataMember(Name = "link")]
        public string Link { get; set; }

    }
}
