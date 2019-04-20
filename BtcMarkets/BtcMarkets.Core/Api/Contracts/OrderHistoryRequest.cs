using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Api.Contracts
{
    [DataContract]
    public class OrderHistoryRequest
    {

        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [DataMember(Name = "instrument")]
        public string Instrument { get; set; }

        [DataMember(Name = "limit")]
        public int Limit { get; set; }

        [DataMember(Name = "since")]
        public long Since { get; set; }
    }

}
