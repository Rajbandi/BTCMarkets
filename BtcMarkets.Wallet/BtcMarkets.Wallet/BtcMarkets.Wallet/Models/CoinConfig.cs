
namespace BtcMarkets.Wallet.Models
{
    using System;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class CoinConfig
    {
        [JsonProperty("lastmodified")]
        public DateTimeOffset Lastmodified { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("marketnews")]
        public MarketNewsItem[] MarketNews { get; set; }

        [JsonProperty("coinmarkets")]
        public CoinMarket[] Coinmarkets { get; set; }
    }

    public partial class CoinMarket
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }
    }

    public partial class CoinConfig
    {
        public static CoinConfig FromJson(string json) => JsonConvert.DeserializeObject<CoinConfig>(json, ConfigDateConverter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this CoinConfig self) => JsonConvert.SerializeObject(self, ConfigDateConverter.Settings);
    }

    internal static class ConfigDateConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
