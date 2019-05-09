
namespace BtcMarkets.Wallet.Models
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [DataContract]
    public partial class CoinConfig
    {
        [JsonProperty("lastmodified")]
        [DataMember(Name = "lastmodified")]
        public DateTimeOffset Lastmodified { get; set; }

        [JsonProperty("version")]
        [DataMember(Name = "version")]
        public string Version { get; set; }

        [JsonProperty("logo")]
        [DataMember(Name = "logo")]
        public string Logo { get; set; }

        [JsonProperty("marketnews")]
        [DataMember(Name ="marketnews")]
        public MarketNewsItem[] MarketNews { get; set; }

        [JsonProperty("coinmarkets")]
        [DataMember(Name = "coinmarkets")]
        public CoinMarket[] Coinmarkets { get; set; }
    }

    [DataContract]
    public partial class CoinMarket
    {
        [JsonProperty("name")]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        [DataMember(Name = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty("image")]
        [DataMember(Name = "image")]
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
