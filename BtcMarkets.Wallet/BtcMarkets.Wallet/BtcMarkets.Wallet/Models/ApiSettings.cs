using Newtonsoft.Json;

namespace BtcMarkets.Wallet.Models
{
    [JsonObject]
    public class ApiSettings
    {
        [JsonProperty("baseurl")]
        public string BaseUrl { get; set; }

        [JsonProperty("apikey")]
        public string ApiKey { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }
    }
}
