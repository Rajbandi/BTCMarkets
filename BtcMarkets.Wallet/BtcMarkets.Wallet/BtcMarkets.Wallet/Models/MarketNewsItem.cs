using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BtcMarkets.Wallet.Models
{
    [JsonObject]
    public class MarketNewsItem
    {

        [JsonProperty("datepublished")]
        public DateTime DatePublished { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

    }
}
