using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Api.Contracts
{
    [DataContract]
    public class AccountBalanceResponse : BaseResponse
    {
        [DataMember(Name = "balances")]
        public List<AccountBalance> Balances { get; set; }
    }
}
