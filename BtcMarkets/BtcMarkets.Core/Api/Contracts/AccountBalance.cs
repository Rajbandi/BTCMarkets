using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Api.Contracts
{
    [DataContract]
    public class AccountBalance
    {
        [DataMember(Name = "balance")]
        public long Balance { get; set; }

        [DataMember(Name = "pendingFunds")]
        public int PendingFunds { get; set; }

        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [DataMember(Name = "balancedecimal")]
        public double BalanceDecimal => Balance > 0 ? (Balance / ApiConstants.CurrencyDecimal) : 0;

    }

}
