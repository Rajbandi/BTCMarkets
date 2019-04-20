using System.Runtime.Serialization;

namespace BtcMarkets.Core.Api.Contracts
{
    [DataContract]
    public class MarketTrade
    {

        [DataMember(Name = "tid")]
        public object Tid { get; set; }

        [DataMember(Name = "amount")]
        public double Amount { get; set; }

        [DataMember(Name = "price")]
        public double Price { get; set; }

        [DataMember(Name = "date")]
        public long Date { get; set; }
    }
}
