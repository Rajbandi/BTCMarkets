using Refit;
using System.Runtime.Serialization;

namespace BtcMarkets.Core.Api.Contracts
{
    [DataContract]
    public class MarketParams : BaseObject
    {
        [AliasAs("indexForward")]
        [DataMember(Name = "indexForward")]
        public bool IndexForward { get; set; }

        [AliasAs("limit")]
        [DataMember(Name = "limit")]
        public int? Limit { get; set; }

        [AliasAs("since")]
        [DataMember(Name = "since")]
        public long? Since { get; set; }

        [AliasAs("sortForward")]
        [DataMember(Name = "sortForward")]
        public bool SortForward { get; set; }
    }
}
