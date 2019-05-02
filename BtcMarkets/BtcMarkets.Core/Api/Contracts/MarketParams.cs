using Refit;
using System.Runtime.Serialization;

namespace BtcMarkets.Core.Api.Contracts
{
    [DataContract]
    public class MarketParams : BaseObject
    {

        public bool? Forward { get; set; }

        public bool? Sort { get; set; }

        [AliasAs("indexForward")]
        [DataMember(Name = "indexForward")]
        public string IndexForward => Forward.HasValue ? Forward.Value.ToString().ToLower() : null;

        [AliasAs("limit")]
        [DataMember(Name = "limit")]
        public int? Limit { get; set; }

        [AliasAs("since")]
        [DataMember(Name = "since")]
        public long? Since { get; set; }

        [AliasAs("sortForward")]
        [DataMember(Name = "sortForward")]
        public string SortForward => Sort.HasValue ? Sort.Value.ToString().ToLower() : null;
    }
}
