using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BtcMarkets.Core.Sockets
{
    [DataContract]
    public class SubscribeMessage
    {

        [DataMember(Name = "messageType")]
        public string MessageType { get; set; } = "subscribe";

        [DataMember(Name = "channels")]
        public IList<string> Channels { get; set; }

        [DataMember(Name = "marketIds")]
        public IList<string> MarketIds { get; set; }

        [DataMember(Name = "signature")]
        public string Signature { get; set; }

        [DataMember(Name = "timestamp")]
        public string Timestamp { get; set; }

        [DataMember(Name = "key")]
        public string Key { get; set; }
    }
}
