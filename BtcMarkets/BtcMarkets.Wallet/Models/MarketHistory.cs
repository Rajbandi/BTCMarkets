using Newtonsoft.Json;
using System;

namespace BtcMarkets.Wallet.Models
{
    [JsonObject]
    public class MarketHistory 
    {
        [JsonProperty("instrument")]
        public string Instrument { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("open")]
        public double Open { get; set; }

        [JsonProperty("close")]
        public double Close { get; set; }

        [JsonProperty("high")]
        public double High { get; set; }

        [JsonProperty("low")]
        public double Low { get; set; }
    }
}
