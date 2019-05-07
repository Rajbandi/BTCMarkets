using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Sockets
{
    [DataContract]
    public class ErrorV2
    {

        [DataMember(Name = "messageType")]
        public string MessageType { get; set; }

        [DataMember(Name = "code")]
        public int Code { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
