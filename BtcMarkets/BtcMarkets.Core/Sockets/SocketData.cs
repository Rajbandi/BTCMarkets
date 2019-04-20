using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace BtcMarkets.Core.Sockets
{
    [JsonObject]
    [DataContract]
    public class SocketData
    {
        [JsonProperty("error")]
        [DataMember(Name = "error")]
        public string Error { get; set; }

        [JsonProperty("success")]
        [DataMember(Name = "success")]
        public bool Success { get; set; }
    }
}
