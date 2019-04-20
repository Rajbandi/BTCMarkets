using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BtcMarkets.Core.Api.Contracts
{
    [DataContract]
    public class MarketOrderBook
    {

        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [DataMember(Name = "instrument")]
        public string Instrument { get; set; }

        [DataMember(Name = "timestamp")]
        public int Timestamp { get; set; }

        [DataMember(Name = "asks")]
        public IList<IList<double>> Asks { get; set; }

        [DataMember(Name = "bids")]
        public IList<IList<double>> Bids { get; set; }
    }

}
