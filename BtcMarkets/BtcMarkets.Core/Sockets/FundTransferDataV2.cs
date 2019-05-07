using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Sockets
{
    [DataContract]
    public class FundTransferDataV2
    {

        [DataMember(Name = "fundtransferId")]
        public int FundtransferId { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "timestamp")]
        public DateTime Timestamp { get; set; }

        [DataMember(Name = "amount")]
        public string Amount { get; set; }

        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [DataMember(Name = "fee")]
        public string Fee { get; set; }

        [DataMember(Name = "messageType")]
        public string MessageType { get; set; }
    }

}
