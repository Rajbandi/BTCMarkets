using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Sockets
{
    [DataContract]
    public class Channel
    {

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "marketIds")]
        public IList<string> MarketIds { get; set; }

    }


    [DataContract]
    public class HeartBeatMessage
    {
        [DataMember(Name = "messageType")]
        public string MessageType { get; set; } = "heartbeat";

        [DataMember(Name = "channels")]
        public List<Channel> Channels { get; set; }
    }
}
